using System.Reflection;
using System.Text;

using AutoMapper;

using Dapper;

using Mi.Domain.Shared.Core;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.Application.System.Impl
{
    public class DictService : IDictService, IScoped
    {
        private readonly IRepository<SysDict> _dictRepo;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUser _miUser;
        private readonly IMapper _mapper;
        private readonly IDapperRepository _dapperRepository;

        public DictService(IRepository<SysDict> dictRepo, IMemoryCache cache, ICurrentUser miUser, IMapper mapper
            , IDapperRepository dapperRepository)
        {
            _dictRepo = dictRepo;
            _cache = cache;
            _miUser = miUser;
            _mapper = mapper;
            _dapperRepository = dapperRepository;
        }

        #region Admin_UI

        public async Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search)
        {
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

            var model = await _dapperRepository.QueryPagedAsync<DictItem>(sql.ToString(), search.Page, search.Size, param: parameters);

            return new ResponseStructure<PagingModel<DictItem>>(true, model);
        }

        public async Task<ResponseStructure> AddOrUpdateDictAsync(DictOperation operation, bool addEnabled = true)
        {
            if (operation.Id <= 0 && addEnabled)
            {
                var dict = _mapper.Map<SysDict>(operation);
                dict.Id = SnowflakeIdHelper.NextId();
                if (dict.ParentId > 0)
                {
                    dict.ParentKey = (await _dictRepo.GetAsync(x => x.Id == dict.ParentId))?.Key;
                }
                await _dictRepo.AddAsync(dict);
            }
            else
            {
                var dict = _mapper.Map<SysDict>(operation);
                if (operation.ParentId.GetValueOrDefault() > 0 && operation.ParentId != dict.ParentId)
                {
                    dict.ParentKey = (await _dictRepo.GetAsync(x => x.Id == dict.ParentId))?.Key;
                }
                await _dictRepo.UpdateAsync(dict);
            }

            _cache.Remove(CacheConst.DICT);

            return ResponseHelper.Success();
        }

        public async Task<ResponseStructure> RemoveDictAsync(IList<string> ids)
        {
            var list = await _dictRepo.GetListAsync(x => ids.Contains(x.Id.ToString()));
            foreach (var item in list)
            {
                item.IsDeleted = 1;
            }

            var rows = await _dictRepo.UpdateRangeAsync(list);

            if (rows > 0)
            {
                _cache.Remove(CacheConst.DICT);
            }
            return ResponseHelper.SuccessOrFail(rows > 0);
        }

        public async Task<ResponseStructure<SysDictFull>> GetAsync(long id)
        {
            var dict = await _dictRepo.GetAsync(x => x.Id == id);
            var model = _mapper.Map<SysDictFull>(dict);

            return new ResponseStructure<SysDictFull>(model);
        }

        public List<SysDictFull> GetAll()
        {
            var dict = _dictRepo.GetListAsync().Result;
            return _mapper.Map<List<SysDictFull>>(dict);
        }

        public async Task<List<Option>> GetParentListAsync()
        {
            var sql = "select Name,Id AS Value from SysDict where IsDeleted = 0 and Id in (select ParentId from SysDict where IsDeleted = 0) ";

            return await _dapperRepository.QueryAsync<Option>(sql);
        }

        #endregion Admin_UI

        #region 公共读写方法，带缓存

        public Task<T> GetAsync<T>(string parentKey) where T : class, new()
        {
            var model = Get<T>(parentKey);

            return model;
        }

        public async Task<T> Get<T>(string parentKey) where T : class, new()
        {
            var dict = (await GetDictionaryCacheAsync()).Where(x => x.ParentKey == parentKey);
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
            var list = await GetDictionaryCacheAsync();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var item = list.FirstOrDefault(x => x.Key == prop.Name);
                if (item != null)
                {
                    item.Value = Convert.ToString(prop.GetValue(model));
                    dict.Add(item);
                }
            }

            var rows = await _dictRepo.UpdateRangeAsync(dict);
            _cache.Remove(CacheConst.FUNCTION);
            return rows > 0;
        }

        public async Task<string> GetStringAsync(string key)
        {
            var str = (await GetDictionaryCacheAsync()).FirstOrDefault(x => x.Key == key)?.Value ?? "";
            return str;
        }

        public async Task<bool> SetAsync(string key, string value, bool autoCreate = true)
        {
            var dict = await _dictRepo.GetAsync(x => x.Key == key);
            var operation = new DictOperation
            {
                Key = key,
                Name = key,
                Id = dict?.Id ?? 0
            };
            var result = await AddOrUpdateDictAsync(operation, autoCreate);

            return result.IsSucceed();
        }

        private async Task<List<SysDict>> GetDictionaryCacheAsync()
        {
            var result = await _cache.GetOrCreate(CacheConst.DICT, async (entry) =>
            {
                var list = await _dictRepo.GetListAsync();
                return list;
            })!;

            return result;
        }

        public async Task<IList<Option>> GetOptionsAsync(string parentKey)
        {
            var dict = (await GetDictionaryCacheAsync()).Where(x => x.ParentKey == parentKey).Select(x => new Option
            {
                Name = x.Name,
                Value = x.Value
            }).ToList();
            return await Task.FromResult(dict);
        }

        public async Task<IList<Option>> GetOptions(string parentKey)
        {
            var dict = (await GetDictionaryCacheAsync()).Where(x => x.ParentKey == parentKey).Select(x => new Option
            {
                Name = x.Name,
                Value = x.Value
            });
            return dict.ToList();
        }

        public async Task<ResponseStructure> SetAsync(Dictionary<string, string> dict)
        {
            var updateList = new List<SysDict>();
            var list = await GetDictionaryCacheAsync();
            foreach (var item in dict)
            {
                var model = list.FirstOrDefault(x => x.Key == item.Key);
                if (model != null)
                {
                    model.Value = item.Value;
                    updateList.Add(model);
                }
            }
            if (updateList.Count > 0) await _dictRepo.UpdateRangeAsync(updateList);
            _cache.Remove(CacheConst.FUNCTION);
            return ResponseHelper.Success();
        }

        public async Task<IList<Option>> GetOptionsAsync()
        {
            var dict = (await GetDictionaryCacheAsync()).Select(x => new Option
            {
                Name = x.Name,
                Value = x.Value
            });
            return dict.ToList();
        }

        #endregion 公共读写方法，带缓存
    }
}