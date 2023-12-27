using System.Text;

using AutoMapper;

using Dapper;

using Simple.Admin.Application.Contracts.System.Models.Dict;
using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.System.Impl
{
    public class DictService : IDictService, IScoped
    {
        private readonly IRepository<SysDict> _dictRepo;
        private readonly IMapper _mapper;
        private readonly IDapperRepository _dapperRepository;
        private readonly IQuickDict _quickDict;

        public DictService(IRepository<SysDict> dictRepo, IMapper mapper
            , IDapperRepository dapperRepository, IQuickDict quickDict)
        {
            _dictRepo = dictRepo;
            _mapper = mapper;
            _dapperRepository = dapperRepository;
            _quickDict = quickDict;
        }

        public async Task<MessageModel<PagingModel<DictItem>>> GetDictListAsync(DictSearch search)
        {
            var sql = new StringBuilder(@"select d.* from SysDict d where d.IsDeleted = 0 ");
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
            if (!string.IsNullOrEmpty(search.Type))
            {
                sql.Append(" and d.Type like @Type ");
                sql.AppendLine(" order by Sort asc ");
                parameters.Add("Type", "%" + search.Type + "%");
            }
            else
            {
                sql.AppendLine(" order by CreatedOn desc ");
            }

            var model = await _dapperRepository.QueryPagedAsync<DictItem>(sql.ToString(), search.Page, search.Size, param: parameters);

            return new MessageModel<PagingModel<DictItem>>(true, model);
        }

        public async Task<MessageModel> RemoveDictAsync(PrimaryKeys input)
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

        public async Task<MessageModel<SysDictFull>> GetAsync(long id)
        {
            var dict = await _dictRepo.GetAsync(x => x.Id == id);
            var model = _mapper.Map<SysDictFull>(dict);

            return new MessageModel<SysDictFull>(model);
        }

        public async Task<List<SysDictFull>> GetAllAsync()
        {
            var dict = await _dictRepo.GetListAsync();
            return _mapper.Map<List<SysDictFull>>(dict);
        }

        public async Task<MessageModel> AddAsync(DictPlus input)
        {
            if (_quickDict[input.Key].IsNotNullOrEmpty()) return Back.Fail($"已存在名为【{input.Key}】的字典");
            var dict = _mapper.Map<SysDict>(input);
            dict.Id = SnowflakeIdHelper.Next();
            var rows = await _dictRepo.AddAsync(dict);
            if (rows > 0)
            {
                _quickDict.Reload();
            }
            return Back.Success();
        }

        public async Task<MessageModel> UpdateAsync(DictEdit input)
        {
            if (_quickDict[input.Key].IsNotNullOrEmpty()) return Back.Fail($"已存在名为【{input.Key}】的字典");
            var dict = await _dictRepo.GetAsync(x => x.Id == input.Id);
            if (dict == null) return Back.NonExist();

            dict.Name = input.Name;
            dict.Key = input.Key;
            dict.Value = input.Value;
            dict.Remark = input.Remark;
            dict.Sort = input.Sort;
            dict.Type = input.Type;

            var rows = await _dictRepo.UpdateAsync(dict);

            if (rows > 0)
            {
                _quickDict.Reload();
            }
            return Back.Success();
        }
    }
}