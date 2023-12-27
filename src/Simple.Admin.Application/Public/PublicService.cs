using Force.DeepCloner;

using Simple.Admin.Application.Contracts.Public;
using Simple.Admin.Application.Contracts.Public.Models;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly ICaptcha _captcha;
        private readonly ITokenManager _tokenManager;

        public PublicService(ICaptcha captcha, ITokenManager tokenManager)
        {
            _captcha = captcha;
            _tokenManager = tokenManager;
        }

        public MessageModel<RefreshTokenResult> GetRefreshTokenResult(RefreshTokenDto dto)
        {
            if (_tokenManager.IsExpired(dto.Token))
            {
                return Back.Fail("token已过期").As<RefreshTokenResult>();
            }
            var person = _tokenManager.GetClaimsPrincipal(dto.Token);
            if (person == null) return Back.Fail("获取用户信息失败").As<RefreshTokenResult>();
            var claims = person.Claims.DeepClone();
            var item = claims.FirstOrDefault(x => x.Type == AuthorizationConst.REFRESH_TYPE);
            if (item != null && item.Value == AuthorizationConst.REFRESH_TOKEN)
            {
                var userClaims = claims.Where(x => x.Type != AuthorizationConst.REFRESH_TYPE);
                var expireTime = DateTime.Now.AddMinutes(Convert.ToInt32(App.Configuration.GetSection("JWT")["Expires"]));
                string token = TokenHelper.GenerateToken(userClaims, expireTime);
                var newExpireTime = expireTime.AddHours(2);
                string refreshToken = TokenHelper.GenerateToken(claims, newExpireTime);
                var res = new RefreshTokenResult
                {
                    accessToken = token,
                    expires = expireTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    refreshToken = refreshToken
                };
                return new MessageModel<RefreshTokenResult>(res);
            }
            return Back.Fail("刷新标识错误").As<RefreshTokenResult>();
        }

        public Task<byte[]> LoginCaptchaAsync(Guid guid)
        {
            var code = StringHelper.GetRandomString(5);
            return _captcha.CreateAsync(guid.ToString(), code, 120, 30);
        }
    }
}