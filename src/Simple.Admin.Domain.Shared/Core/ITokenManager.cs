using System.Security.Claims;

namespace Simple.Admin.Domain.Shared.Core
{
    public interface ITokenManager : ISingleton
    {
        /// <summary>
        /// 添加token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expire">过期时间</param>
        void AddToken(string token, DateTime expire);

        /// <summary>
        /// 判断token是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsExpired(string token);

        /// <summary>
        /// 从Token中获取用户身份
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal? GetClaimsPrincipal(string token);
    }
}