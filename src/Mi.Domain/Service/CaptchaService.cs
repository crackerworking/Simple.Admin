using Mi.Domain.Helper;
using Mi.Domain.Shared.Core;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.Domain.Service
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
            _memoryCache.Set(cacheKey, verifyCode);
            return Task.FromResult(bytes);
        }

        public Task<bool> ValidateAsync(string cacheKey, string verifyCode)
        {
            var flag = _memoryCache.TryGetValue<string>(cacheKey, out var code) && code == verifyCode;
            return Task.FromResult(flag);
        }
    }
}