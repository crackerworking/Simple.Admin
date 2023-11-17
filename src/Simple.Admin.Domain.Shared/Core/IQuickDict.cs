using System.Runtime.CompilerServices;

using Simple.Admin.Domain.Shared.Options;

namespace Simple.Admin.Domain.Shared.Core
{
    /// <summary>
    /// 全局字典 IQuickDict
    /// </summary>
    public interface IQuickDict
    {
        /// <summary>
        /// 获取字典value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; }

        /// <summary>
        /// 读取字典value
        /// </summary>
        /// <param name="key">字典key</param>
        /// <returns></returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 读取字典子集
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetManyAsync(string parentKey);

        /// <summary>
        /// 读取字典子集，赋值到模型
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        Task<T> GetManyAsync<T>(string parentKey);

        /// <summary>
        /// 读取字典子集，组装成option
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        Task<List<Option>> GetOptionsAsync(string parentKey);

        /// <summary>
        /// 更新字典value
        /// </summary>
        /// <param name="key">字典key</param>
        /// <param name="value">字典value</param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, string value);

        /// <summary>
        /// 批量更新字典value
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        Task<bool> SetAsync(Dictionary<string, string> dict);

        /// <summary>
        /// 重新加载
        /// </summary>
        void Reload();
    }
}