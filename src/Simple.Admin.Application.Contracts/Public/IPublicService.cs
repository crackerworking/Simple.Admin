using Simple.Admin.Domain.Shared.Models.UI;

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
    }
}