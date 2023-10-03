using Mi.Application.Contracts.System.Models;
using Mi.Application.Contracts.System.Models.Result;
using Mi.Domain.Shared.Options;

namespace Mi.Application.Contracts.System
{
    public interface IDictService
    {
        #region Admin_UI

        List<SysDictFull> GetAll();

        Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        Task<ResponseStructure> AddOrUpdateDictAsync(DictOperation operation, bool addEnabled = true);

        Task<ResponseStructure> RemoveDictAsync(IList<string> ids);

        Task<ResponseStructure<SysDictFull>> GetAsync(long id);

        Task<List<Option>> GetParentListAsync();

        #endregion Admin_UI

        #region 公共读写方法，带缓存

        Task<T> GetAsync<T>(string parentKey) where T : class, new();

        Task<bool> SetAsync<T>(T model) where T : class, new();

        Task<string> GetStringAsync(string key);

        Task<bool> SetAsync(string key, string value, bool autoCreate = true);

        Task<IList<Option>> GetOptionsAsync(string parentKey);

        Task<ResponseStructure> SetAsync(Dictionary<string, string> dict);

        #endregion 公共读写方法，带缓存
    }
}