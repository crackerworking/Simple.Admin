using System.Data;
using System.Linq.Expressions;

using AutoMapper;

using Mi.Domain.Entities.System.Enum;
using Mi.Domain.Extension;

namespace Mi.Application.System.Impl
{
    public class FunctionService : IFunctionService, IScoped
    {
        private readonly IMapper _mapper;
        private readonly ResponseStructure _message;
        private readonly ICurrentUser _miUser;
        private readonly IRepository<SysFunction> _functionRepo;
        private readonly IMemoryCache _cache;

        public FunctionService(IMapper mapper, ResponseStructure message, ICurrentUser miUser
            , IRepository<SysFunction> functionRepo
            , IMemoryCache cache)
        {
            _mapper = mapper;
            _message = message;
            _miUser = miUser;
            _functionRepo = functionRepo;
            _cache = cache;
        }

        private IList<SysFunctionFull> _allFunctions => GetFunctionsCacheAsync().ConfigureAwait(false).GetAwaiter().GetResult();

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
                func.CreatedOn = DateTime.Now;
                func.Id = SnowflakeIdHelper.NextId();
                func.Node = (EnumTreeNode)CheckFunctionNode(_mapper.Map<SysFunctionFull>(func));
                if (func.ParentId > 0)
                {
                    await UpdateParentNodeAsync(func.ParentId, func.Id);
                }
                await _functionRepo.AddAsync(func);
            }
            else
            {
                var func = _mapper.Map<SysFunction>(operation);
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
                func.Node = (EnumTreeNode)CheckFunctionNode(_mapper.Map<SysFunctionFull>(func));
                await _functionRepo.UpdateAsync(func);
            }
            RemoveCache();

            return ResponseHelper.Success();
        }

        private async Task UpdateParentNodeAsync(long parentId, long childId, long rawParentId = default)
        {
            var parent = await _functionRepo.GetAsync(x => x.Id == parentId);
            parent.Children += "," + childId;
            parent.Node = (EnumTreeNode)CheckFunctionNode(_mapper.Map<SysFunctionFull>(parent));
            parent.Children = parent.Children.Trim(',');
            if (rawParentId > 0)
            {
                var raw = await _functionRepo.GetAsync(x => x.Id == rawParentId);
                raw.Children = (raw.Children ?? "").Replace(childId.ToString(), "").Trim(',');
                raw.Node = (EnumTreeNode)CheckFunctionNode(_mapper.Map<SysFunctionFull>(raw));
                await _functionRepo.UpdateAsync(raw);
            }

            await _functionRepo.UpdateAsync(parent);
        }

        public int CheckFunctionNode(SysFunctionFull node)
        {
            var hasChildren = !string.IsNullOrEmpty(node.Children) && node.Children.Split(",").Length > 0;
            var hasParent = node.ParentId > 0;

            if (hasParent && hasChildren)
                return (int)EnumTreeNode.ChildNode;
            else if (hasParent && !hasChildren)
                return (int)EnumTreeNode.LeafNode;

            return (int)EnumTreeNode.RootNode;
        }

        public async Task<SysFunctionFull> GetAsync(long id)
        {
            var model = await _functionRepo.GetAsync(x => x.Id == id);
            return _mapper.Map<SysFunctionFull>(model);
        }

        public async Task<ResponseStructure<IList<FunctionItem>>> GetFunctionListAsync(FunctionSearch search)
        {
            var exp = PredicateBuilder.Instance.Create<SysFunctionFull>()
                .AndIf(!string.IsNullOrEmpty(search.FunctionName), x => x.FunctionName.Contains(search.FunctionName!))
                .AndIf(!string.IsNullOrEmpty(search.Url), x => x.Url != null && x.Url.Contains(search.Url!));

            var searchList = _allFunctions.Where(exp.Compile()).OrderBy(x => x.Sort);
            var flag = exp.Body.NodeType == ExpressionType.AndAlso;
            var topLevel = flag ? searchList : _allFunctions.Where(x => x.Node == (int)EnumTreeNode.RootNode).OrderBy(x => x.Sort);
            var list = topLevel.Select(x => new FunctionItem
            {
                FunctionName = x.FunctionName,
                Icon = x.Icon,
                Url = x.Url,
                FunctionType = (int)x.FunctionType,
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
            var children = _allFunctions.Where(x => x.Node != (int)EnumTreeNode.RootNode && x.ParentId == id).OrderBy(x => x.Sort);
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
            var topLevels = _allFunctions.Where(x => x.Node == (int)EnumTreeNode.RootNode).OrderBy(x => x.Sort);
            return topLevels.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }

        private IList<TreeOption> GetFunctionChildNode(long id)
        {
            var children = _allFunctions.Where(x => x.Node != (int)EnumTreeNode.RootNode && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }

        public async Task<ResponseStructure> RemoveFunctionAsync(IList<long> ids)
        {
            if (ids.Count <= 0) return ResponseHelper.Fail("id不能为空");

            var funcs = await _functionRepo.GetListAsync(x => ids.Contains(x.Id));
            foreach (var item in funcs)
            {
                item.IsDeleted = 1;
            }
            await _functionRepo.UpdateRangeAsync(funcs);
            RemoveCache();

            return ResponseHelper.Success();
        }

        public async Task<IList<SysFunctionFull>> GetFunctionsCacheAsync()
        {
            return await _cache.GetOrCreate(CacheConst.FUNCTION, async (entry) =>
            {
                var list = await _functionRepo.GetListAsync();
                _cache.Set(CacheConst.FUNCTION, list.ToList(), CacheConst.Week);
                return _mapper.Map<IList<SysFunctionFull>>(list);
            })!;
        }

        private void RemoveCache()
        {
        }

        public async Task<IList<string>> GetAllIdsAsync()
        {
            return (await GetFunctionsCacheAsync()).Select(x => x.Id.ToString()).ToList();
        }
    }
}