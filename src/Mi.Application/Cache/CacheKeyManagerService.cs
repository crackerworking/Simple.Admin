using System.Text.Json;

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

        public async Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(string? vague, int cacheType = 1)
        {
            var keys = _cache.GetCacheKeys();
            var list = keys.Select(k => new Option
            {
                Name = k,
                Value = k
            }).ToList();

            return await Task.FromResult(new ResponseStructure<IList<Option>>(list));
        }

        public async Task<ResponseStructure<string>> GetDataAsync(string key)
        {
            _cache.TryGetValue(key, out var value);
            var str = JsonSerializer.Serialize(value ?? "");

            return await Task.FromResult(new ResponseStructure<string>(str));
        }

        public Task<ResponseStructure> RemoveKeyAsync(string key)
        {
            _cache.Remove(key);

            return Task.FromResult(ResponseHelper.Success());
        }
    }
}