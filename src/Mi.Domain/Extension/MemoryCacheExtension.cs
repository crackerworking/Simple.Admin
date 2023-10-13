using System.Collections;
using System.Reflection;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.Domain.Extension
{
    public static class MemoryCacheExtension
    {
        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCacheKeys(this IMemoryCache _cache)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var coherentState = _cache.GetType().GetField("_coherentState", flags)!.GetValue(_cache) ?? throw new Exception("获取_coherentState失败");
            var entries = coherentState.GetType().GetField("_entries", flags)!.GetValue(coherentState);
            var keys = new List<string>();
            if (entries is not IDictionary cacheItems) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString()!);
            }
            return keys;
        }
    }
}