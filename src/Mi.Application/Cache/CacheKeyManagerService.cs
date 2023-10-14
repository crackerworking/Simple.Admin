using System.Text.Json;

using Mi.Application.Contracts.Cache;
using Mi.Application.Contracts.Cache.Models;
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

        public async Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(CacheKeySearch input)
        {
            var keys = _cache.GetCacheKeys();
            var list = keys.Select(k => new Option
            {
                Name = k,
                Value = k
            }).ToList();

            return await Task.FromResult(new ResponseStructure<IList<Option>>(list));
        }

        public async Task<ResponseStructure<string>> GetDataAsync(CacheKeyIn input)
        {
            _cache.TryGetValue(input.key, out var value);
            var str = JsonSerializer.Serialize(value ?? "");

            return await Task.FromResult(new ResponseStructure<string>(str));
        }

        public Task<ResponseStructure> RemoveKeyAsync(CacheKeyIn input)
        {
            _cache.Remove(input.key);

            return Task.FromResult(ResponseHelper.Success());
        }
    }
}