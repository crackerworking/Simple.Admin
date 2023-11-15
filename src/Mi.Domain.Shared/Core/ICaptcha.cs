namespace Mi.Domain.Shared.Core
{
    public interface ICaptcha : ISingleton
    {
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        Task<byte[]> CreateAsync(string cacheKey, string verifyCode, int width, int height);

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns>true正确</returns>
        Task<bool> ValidateAsync(string cacheKey, string verifyCode);
    }
}