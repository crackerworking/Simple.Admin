using Mi.Application.Contracts.System.Models.Dict;
using Mi.Domain.Shared.Options;

namespace Mi.Application.Contracts.System
{
    public interface IDictService
    {
        List<SysDictFull> GetAll();

        Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        Task<ResponseStructure> AddOrUpdateDictAsync(DictOperation operation, bool addEnabled = true);

        Task<ResponseStructure> RemoveDictAsync(IList<string> ids);

        Task<ResponseStructure<SysDictFull>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();
    }
}