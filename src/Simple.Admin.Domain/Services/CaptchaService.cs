using Microsoft.Extensions.Caching.Memory;

using Simple.Admin.Domain.Helper;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain.Services
{
    internal class CaptchaService : ICaptcha
    {
        private readonly IMemoryCache _memoryCache;

        public CaptchaService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<byte[]> CreateAsync(string cacheKey, string verifyCode, int width, int height)
        {
            var bytes = DrawingHelper.CreateByteByImgVerifyCode(verifyCode, width, height);
            _memoryCache.Set(cacheKey, verifyCode, TimeSpan.FromMinutes(1));
            return Task.FromResult(bytes);
        }

        public Task<bool> ValidateAsync(string cacheKey, string verifyCode)
        {
            var flag = _memoryCache.TryGetValue<string>(cacheKey, out var code) && code != null && code.Equals(verifyCode, StringComparison.CurrentCultureIgnoreCase);
            return Task.FromResult(flag);
        }
    }
}