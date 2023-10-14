using Mi.Application.Contracts.Cache.Models;

namespace Mi.Application.Contracts.Cache
{
    public interface ICacheKeyManagerService
    {
        Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(CacheKeySearch input);

        Task<ResponseStructure> RemoveKeyAsync(CacheKeyIn input);

        Task<ResponseStructure<string>> GetDataAsync(CacheKeyIn input);
    }
}
