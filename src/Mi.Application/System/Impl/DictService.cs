using System.Text;

using AutoMapper;

using Dapper;
using Mi.Application.Contracts.System.Models.Dict;
using Mi.Domain.Shared.Core;

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
    }
}