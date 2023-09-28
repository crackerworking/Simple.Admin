using Mi.Core.API;
using Mi.Core.Factory;
using Mi.IService.Cache;

using Newtonsoft.Json;

namespace Mi.Application.Cache
{
    public class CacheKeyManagerService : ICacheKeyManagerService, IScoped
    {
        private readonly MemoryCacheFactory _cache;
        private readonly ResponseStructure _msg;

        public CacheKeyManagerService(MemoryCacheFactory cache, ResponseStructure msg)
        {
            _cache = cache;
            _msg = msg;
        }

        public Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(string? vague, int cacheType = 1)
        {
            var list = new List<Option>();
            if (cacheType == 1)
            {
                list = _cache.GetCacheKeys().OrderBy(x=>x).Select(x=>new Option{ Name = x}).ToList();
                if (!string.IsNullOrEmpty(vague))
                {
                    list = list.Where(x => x.Name!.Contains(vague)).ToList();
                }
            }
            return Task.FromResult(new ResponseStructure<IList<Option>>(list));
        }

        public Task<ResponseStructure<string>> GetDataAsync(string key)
        {
            var str = string.Empty;
            if (_cache.Exists(key))
            {
                str = JsonConvert.SerializeObject(_cache.Get<dynamic>(key));
            }
            return Task.FromResult(new ResponseStructure<string>(str));
        }

        public Task<ResponseStructure> RemoveKeyAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(_msg.Success());
        }
    }
}
