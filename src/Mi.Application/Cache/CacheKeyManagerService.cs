using Mi.Application.Contracts.Cache;
using Mi.Domain.Shared.Core;

namespace Mi.Application.Cache
{
    public class CacheKeyManagerService : ICacheKeyManagerService, IScoped
    {
        private readonly IMemoryCache _cache;

        public CacheKeyManagerService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(string? vague, int cacheType = 1)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseStructure<string>> GetDataAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseStructure> RemoveKeyAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}