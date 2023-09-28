using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using AutoMapper;
using Mi.Core.API;
using Mi.Core.Factory;
using Mi.Core.GlobalVar;
using Mi.Core.Helper;
using Mi.Entity.System.Enum;
using Mi.IService.System.Models.Result;

namespace Mi.Application.System
{
    public class FunctionService : IFunctionService, IScoped
    {
        private readonly IMapper _mapper;
        private readonly ResponseStructure _message;
        private readonly IMiUser _miUser;
        private readonly IFunctionRepository _functionRepository;
        private readonly MemoryCacheFactory _cache;

        public FunctionService(IMapper mapper, ResponseStructure message, IMiUser miUser
            , IFunctionRepository functionRepository
            , MemoryCacheFactory cache)
        {
            _mapper = mapper;
            _message = message;
            _miUser = miUser;
            _functionRepository = functionRepository;
            _cache = cache;
        }

        private IList<SysFunction> _allFunctions => GetFunctionsCache();

        public async Task<ResponseStructure> AddOrUpdateFunctionAsync(FunctionOperation operation)
        {
            if (string.IsNullOrWhiteSpace(operation.Icon) && operation.FunctionType == (int)EnumFunctionType.Menu)
            {
                operation.Icon = "iconfont mi-iconfonticon";
            }
            if (operation.Id <= 0)
            {
                var func = _mapper.Map<SysFunction>(operation);
                func.CreatedBy = _miUser.UserId;
                func.CreatedOn = TimeHelper.LocalTime();
                func.Id = IdHelper.SnowflakeId();
                func.Node = CheckFunctionNode(func);
                if (func.ParentId > 0)
                {
                    await UpdateParentNodeAsync(func.ParentId, func.Id);
                }
                await _functionRepository.AddAsync(func);
            }
            else
            {
                var func = _functionRepository.Get(operation.Id);
                func.Icon = operation.Icon;
                if (operation.ParentId > 0 && operation.ParentId != func.ParentId)
                {
                    await UpdateParentNodeAsync(operation.ParentId, func.Id, func.ParentId);
                }
                func.FunctionName = operation.FunctionName;
                func.Url = operation.Url;
                func.AuthorizationCode = operation.AuthorizationCode;
                func.ParentId = operation.ParentId;
                func.Sort = operation.Sort;
                func.FunctionType = (EnumFunctionType)operation.FunctionType;
                func.ModifiedBy = _miUser.UserId;
                func.ModifiedOn = TimeHelper.LocalTime();
                func.Node = CheckFunctionNode(func);
                await _functionRepository.UpdateAsync(func);
            }
            RemoveCache();

            return _message.Success();
        }

        private async Task UpdateParentNodeAsync(long parentId, long childId, long rawParentId = default)
        {
            var parent = await _functionRepository.GetAsync(parentId);
            parent.Children += "," + childId;
            parent.Node = CheckFunctionNode(parent);
            parent.Children = parent.Children.Trim(',');
            parent.ModifiedBy = _miUser.UserId;
            parent.ModifiedOn = TimeHelper.LocalTime();
            if (rawParentId > 0)
            {
                var raw = await _functionRepository.GetAsync(rawParentId);
                raw.Children = (raw.Children ?? "").Replace(childId.ToString(), "").Trim(',');
                raw.Node = CheckFunctionNode(raw);
                raw.ModifiedBy = _miUser.UserId;
                raw.ModifiedOn = TimeHelper.LocalTime();
                await _functionRepository.UpdateAsync(raw);
            }

            await _functionRepository.UpdateAsync(parent);
        }

        public EnumTreeNode CheckFunctionNode(SysFunction node)
        {
            var hasChildren = !string.IsNullOrEmpty(node.Children) && node.Children.Split(",").Length > 0;
            var hasParent = node.ParentId > 0;

            if (hasParent && hasChildren)
                return EnumTreeNode.ChildNode;
            else if (hasParent && !hasChildren)
                return EnumTreeNode.LeafNode;

            return EnumTreeNode.RootNode;
        }

        public Task<SysFunction> GetAsync(long id)
        {
            return _functionRepository.GetAsync(id);
        }

        public async Task<ResponseStructure<IList<FunctionItem>>> GetFunctionListAsync(FunctionSearch search)
        {
            var exp = ExpressionCreator.New<SysFunction>()
                .AndIf(!string.IsNullOrEmpty(search.FunctionName), x => x.FunctionName.Contains(search.FunctionName!))
                .AndIf(!string.IsNullOrEmpty(search.Url), x => x.Url != null && x.Url.Contains(search.Url!));

            var searchList = _allFunctions.Where(exp.Compile()).OrderBy(x => x.Sort);
            var flag = exp.Body.NodeType == ExpressionType.AndAlso;
            var topLevel = flag ? searchList : _allFunctions.Where(x => x.Node == EnumTreeNode.RootNode).OrderBy(x => x.Sort);
            var list = topLevel.Select(x => new FunctionItem
            {
                FunctionName = x.FunctionName,
                Icon = x.Icon,
                Url = x.Url,
                FunctionType = x.FunctionType,
                AuthorizationCode = x.AuthorizationCode,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Id = x.Id,
                Children = GetFuncChildNode(x.Id)
            }).ToList();

            return new ResponseStructure<IList<FunctionItem>>(await Task.FromResult(list));
        }

        private IList<FunctionItem> GetFuncChildNode(long id)
        {
            var children = _allFunctions.Where(x => x.Node != EnumTreeNode.RootNode && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new FunctionItem
            {
                FunctionName = x.FunctionName,
                Icon = x.Icon,
                Url = x.Url,
                FunctionType = x.FunctionType,
                AuthorizationCode = x.AuthorizationCode,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Id = x.Id,
                Children = GetFuncChildNode(x.Id)
            }).ToList();
        }

        public IList<TreeOption> GetFunctionTree()
        {
            var topLevels = _allFunctions.Where(x => x.Node == EnumTreeNode.RootNode).OrderBy(x => x.Sort);
            return topLevels.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }

        private IList<TreeOption> GetFunctionChildNode(long id)
        {
            var children = _allFunctions.Where(x => x.Node != EnumTreeNode.RootNode && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }

        public async Task<ResponseStructure> RemoveFunctionAsync(IList<long> ids)
        {
            if (ids.Count <= 0) return _message.Fail("id不能为空");

            var funcs = await _functionRepository.GetAllAsync(x => ids.Contains(x.Id));
            foreach (var item in funcs)
            {
                item.IsDeleted = 1;
                item.ModifiedBy = _miUser.UserId;
                item.ModifiedOn = TimeHelper.LocalTime();
            }
            await _functionRepository.UpdateManyAsync(funcs);
            RemoveCache();

            return _message.Success();
        }

        public IList<SysFunction> GetFunctionsCache()
        {
            var data = _cache.Get<List<SysFunction>>(CacheConst.FUNCTION);
            if (data == null)
            {
                var list = _functionRepository.GetAll();
                _cache.Set(CacheConst.FUNCTION, list.ToList(), CacheConst.Week);
                return list;
            }
            return data;
        }

        private void RemoveCache()
        {
            var keys = _cache.GetCacheKeys().Where(x => Regex.IsMatch(x, StringHelper.UserCachePattern()) || x.Contains(StringHelper.UserKey("", AuthorizationConst.SUPER_ADMIN))).ToList();
            keys.Add(CacheConst.FUNCTION);
            _cache.RemoveAll(keys);
        }

        public IList<string> GetAllIds()
        {
            return GetFunctionsCache().Select(x=>x.Id.ToString()).ToList();
        }
    }
}