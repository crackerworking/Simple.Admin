using Mi.Application.Contracts.System.Models.Dict;

namespace Mi.Application.Contracts.System
{
    public interface IDictService
    {
        Task<List<SysDictFull>> GetAllAsync();

        Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        Task<ResponseStructure> AddAsync(DictPlus input);

        Task<ResponseStructure> UpdateAsync(DictEdit input);

        Task<ResponseStructure> RemoveDictAsync(PrimaryKeys input);

        Task<ResponseStructure<SysDictFull>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();
    }
}