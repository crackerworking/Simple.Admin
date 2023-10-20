using Mi.Application.Contracts.Cache.Models;

namespace Mi.Application.Contracts.Cache
{
    public interface ICacheKeyManagerService
    {
        /// <summary>
        /// 所有缓存key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(CacheKeySearch input);

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> RemoveKeyAsync(CacheKeyIn input);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure<string>> GetDataAsync(CacheKeyIn input);
    }
}
