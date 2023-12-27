using Simple.Admin.Application.Contracts.Public.Models;

namespace Simple.Admin.Application.Contracts.Public
{
    public interface IPublicService
    {
        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <param name="guid">验证码缓存key</param>
        /// <returns></returns>
        Task<byte[]> LoginCaptchaAsync(Guid guid);

        /// <summary>
        /// 获取刷新token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        MessageModel<RefreshTokenResult> GetRefreshTokenResult(RefreshTokenDto dto);
    }
}