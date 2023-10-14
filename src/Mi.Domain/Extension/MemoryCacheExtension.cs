using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// 通过正则表达式移除缓存
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="pattern"></param>
        public static void RemoveByPattern(this IMemoryCache _cache, string pattern)
        {
            var keys = _cache.GetCacheKeys();
            foreach (var key in keys.Where(x => Regex.IsMatch(x, pattern)))
            {
                _cache.Remove(key);
            }
        }
    }
}