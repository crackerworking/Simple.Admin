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
        private readonly IMapper _mapper;
        private readonly IDapperRepository _dapperRepository;
        private readonly IQuickDict _quickDict;

        public DictService(IRepository<SysDict> dictRepo, IMapper mapper
            , IDapperRepository dapperRepository,IQuickDict quickDict)
        {
            _dictRepo = dictRepo;
            _mapper = mapper;
            _dapperRepository = dapperRepository;
            _quickDict = quickDict;
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
            if (search.ParentId.HasValue && search.ParentId > 0)
            {
                sql.Append(" and d.ParentId = @parentId ");
                sql.AppendLine(" order by Sort asc ");
                parameters.Add("parentId", search.ParentId);
            }
            else
            {
                sql.AppendLine(" order by CreatedOn desc ");
            }

            var model = await _dapperRepository.QueryPagedAsync<DictItem>(sql.ToString(), search.Page, search.Size, param: parameters);

            return new ResponseStructure<PagingModel<DictItem>>(true, model);
        }

        public async Task<ResponseStructure> RemoveDictAsync(PrimaryKeys input)
        {
            var list = await _dictRepo.GetListAsync(x => input.array_id.Contains(x.Id));
            foreach (var item in list)
            {
                item.IsDeleted = 1;
            }

            var rows = await _dictRepo.UpdateRangeAsync(list);
            if (rows > 0)
            {
                _quickDict.Reload();
            }

            return Back.SuccessOrFail(rows > 0);
        }

        public async Task<ResponseStructure<SysDictFull>> GetAsync(long id)
        {
            var dict = await _dictRepo.GetAsync(x => x.Id == id);
            var model = _mapper.Map<SysDictFull>(dict);

            return new ResponseStructure<SysDictFull>(model);
        }

        public async Task<List<SysDictFull>> GetAllAsync()
        {
            var dict = await _dictRepo.GetListAsync();
            return _mapper.Map<List<SysDictFull>>(dict);
        }

        public async Task<List<Option>> GetParentListAsync()
        {
            var sql = "select Name,Id AS Value from SysDict where IsDeleted = 0 and Id in (select ParentId from SysDict where IsDeleted = 0) ";

            return await _dapperRepository.QueryAsync<Option>(sql);
        }

        public async Task<ResponseStructure> AddAsync(DictPlus input)
        {
            var dict = _mapper.Map<SysDict>(input);
            dict.Id = SnowflakeIdHelper.Next();
            if (dict.ParentId > 0)
            {
                dict.ParentKey = (await _dictRepo.GetAsync(x => x.Id == dict.ParentId))?.Key;
            }
            var rows = await _dictRepo.AddAsync(dict);
            if (rows > 0)
            {
                _quickDict.Reload();
            }
            return Back.Success();
        }

        public async Task<ResponseStructure> UpdateAsync(DictEdit input)
        {
            var dict = await _dictRepo.GetAsync(x => x.Id == input.Id);
            if (dict == null) return Back.NonExist();

            dict.Name = input.Name;
            dict.Key = input.Key;
            dict.Value = input.Value;
            dict.Remark = input.Remark;
            dict.Sort = input.Sort;
            dict.ParentId = input.ParentId;
            dict.ParentKey = input.ParentKey;

            var rows = await _dictRepo.UpdateAsync(dict);

            if (rows > 0)
            {
                _quickDict.Reload();
            }
            return Back.Success();
        }
    }
}