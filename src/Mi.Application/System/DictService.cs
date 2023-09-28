using System.Reflection;
using System.Text;

using AutoMapper;

using Dapper;
using Mi.Core.API;
using Mi.Core.Factory;
using Mi.Core.GlobalVar;
using Mi.Core.Helper;
using Mi.Core.Service;
using Mi.IService.System.Models.Result;
using Mi.Repository.BASE;

namespace Mi.Application.System
{
    public class DictService : IDictService, IScoped
    {
        private readonly IDictRepository _dictRepository;
        private readonly MemoryCacheFactory _cache;
        private readonly IMiUser _miUser;
        private readonly IMapper _mapper;
        private readonly ResponseStructure _message;

        public DictService(IDictRepository dictRepository, MemoryCacheFactory cache, IMiUser miUser, IMapper mapper
            , ResponseStructure message)
        {
            _dictRepository = dictRepository;
            _cache = cache;
            _miUser = miUser;
            _mapper = mapper;
            _message = message;
        }

        #region Admin_UI

        public async Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search)
        {
            var repo = DotNetService.Get<Repository<DictItem>>();
            var sql = new StringBuilder(@"select d.*,(select count(*) from SysDict where id = d.ParentId) ChildCount,(select name from SysDict where id=d.ParentId) ParentName from SysDict d where d.IsDeleted = 0 ");
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(search.Vague))
            {
                sql.Append(" and ( d.name like @text or d.key like @text )");
                parameters.Add("text", "%" + search.Vague + "%");
            }
            if (!string.IsNullOrEmpty(search.Remark))
            {
                sql.Append(" and d.remark like @remark ");
                parameters.Add("remark", "%" + search.Remark + "%");
            }
            var orderBy = "";
            if (search.ParentId.HasValue && search.ParentId > 0)
            {
                sql.Append(" and d.ParentId = @parentId ");
                orderBy = " Sort asc,";
                parameters.Add("parentId", search.ParentId);
            }
            sql.AppendFormat(" order by {0} CreatedOn desc ", orderBy);

            return new ResponseStructure<PagingModel<DictItem>>(true, await repo.GetPagingAsync(search, sql.ToString(), parameters));
        }

        public async Task<ResponseStructure> AddOrUpdateDictAsync(DictOperation operation, bool addEnabled = true)
        {
            if (operation.Id <= 0 && addEnabled)
            {
                var dict = _mapper.Map<SysDict>(operation);
                dict.Id = IdHelper.SnowflakeId();
                dict.CreatedBy = _miUser.UserId;
                dict.CreatedOn = TimeHelper.LocalTime();
                if (dict.ParentId > 0)
                {
                    dict.ParentKey = _dictRepository.Get(dict.ParentId).Key;
                }
                await _dictRepository.AddAsync(dict);
            }
            else
            {
                var dict = await _dictRepository.GetAsync(operation.Id);
                if (operation.ParentId > 0 && operation.ParentId != dict.ParentId)
                {
                    dict.ParentKey = _dictRepository.Get(dict.ParentId).Key;
                }
                operation.CopyTo(dict, "Id");
                await _dictRepository.UpdateAsync(dict);
            }

            _cache.Remove(CacheConst.DICT);

            return _message.Success();
        }

        public async Task<ResponseStructure> RemoveDictAsync(IList<string> ids)
        {
            var list = await _dictRepository.GetAllAsync(x => ids.Contains(x.Id.ToString()));
            var now = TimeHelper.LocalTime();
            foreach (var item in list)
            {
                item.ModifiedBy = _miUser.UserId;
                item.ModifiedOn = now;
                item.IsDeleted = 1;
            }

            var flag = await _dictRepository.UpdateManyAsync(list);

            if (flag)
            {
                _cache.Remove(CacheConst.DICT);
            }
            return _message.SuccessOrFail(flag);
        }

        public async Task<ResponseStructure<SysDict>> GetAsync(long id)
        {
            var dict = await _dictRepository.GetAsync(id);

            return new ResponseStructure<SysDict>(dict);
        }

        public List<SysDict> GetAll() => _dictRepository.GetAll().ToList();

        public async Task<List<Option>> GetParentListAsync()
        {
            var sql = "select Name,Id AS Value from SysDict where IsDeleted = 0 and Id in (select ParentId from SysDict where IsDeleted = 0) ";
            var repo = DotNetService.Get<Repository<Option>>();

            return await repo.GetListAsync(sql);
        }

        #endregion Admin_UI

        #region 公共读写方法，带缓存

        public Task<T> GetAsync<T>(string parentKey) where T : class, new()
        {
            var model = Get<T>(parentKey);

            return Task.FromResult(model);
        }

        public T Get<T>(string parentKey) where T : class, new()
        {
            var dict = GetDictionaryCache().Where(x => x.ParentKey == parentKey);
            var model = Activator.CreateInstance<T>();

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var item = dict.FirstOrDefault(x => x.Key == prop.Name);
                if (item != null)
                {
                    prop.SetValue(model, Convert.ChangeType(item.Value, prop.PropertyType));
                }
            }

            return model;
        }

        public async Task<bool> SetAsync<T>(T model) where T : class, new()
        {
            var dict = new List<SysDict>();
            var list = GetDictionaryCache();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var item = list.FirstOrDefault(x => x.Key == prop.Name);
                if (item != null)
                {
                    item.Value = Convert.ToString(prop.GetValue(model));
                    dict.Add(item);
                }
            }

            var flag = await _dictRepository.UpdateManyAsync(dict);
            _cache.Remove(CacheConst.FUNCTION);
            return flag;
        }

        public Task<string> GetStringAsync(string key)
        {
            var str = GetDictionaryCache().FirstOrDefault(x => x.Key == key)?.Value ?? "";
            return Task.FromResult(str);
        }

        public async Task<bool> SetAsync(string key, string value, bool autoCreate = true)
        {
            var dict = await _dictRepository.GetAsync(x => x.Key == key);
            var operation = new DictOperation
            {
                Key = key,
                Name = key,
                Id = dict?.Id ?? 0
            };
            var result = await AddOrUpdateDictAsync(operation, autoCreate);

            return result.EnsureSuccess();
        }

        private List<SysDict> GetDictionaryCache()
        {
            if (_cache.Exists(CacheConst.DICT)) return _cache.Get<List<SysDict>>(CacheConst.DICT) ?? new List<SysDict>();

            var list = _dictRepository.GetAll().ToList();
            _cache.Set(CacheConst.DICT, list, CacheConst.Week);
            return list;
        }

        public async Task<IList<Option>> GetOptionsAsync(string parentKey)
        {
            var dict = GetDictionaryCache().Where(x => x.ParentKey == parentKey).Select(x => new Option
            {
                Name = x.Name,
                Value = x.Value
            }).ToList();
            return await Task.FromResult(dict);
        }

        public IList<Option> GetOptions(string parentKey)
        {
            var dict = GetDictionaryCache().Where(x => x.ParentKey == parentKey).Select(x => new Option
            {
                Name = x.Name,
                Value = x.Value
            });
            return dict.ToList();
        }

        public async Task<ResponseStructure> SetAsync(Dictionary<string, string> dict)
        {
            var updateList = new List<SysDict>();
            var list = GetDictionaryCache();
            foreach (var item in dict)
            {
                var model = list.FirstOrDefault(x => x.Key == item.Key);
                if(model != null)
                {
                    model.Value = item.Value;
                    updateList.Add(model);
                }
            }
            if(updateList.Count > 0) await _dictRepository.UpdateManyAsync(updateList);
            _cache.Remove(CacheConst.FUNCTION);
            return _message.Success();
        }

        #endregion
    }
}