namespace Mi.Domain.Shared.Core
{
    public interface ICaptcha : ISingleton
    {
        Task<byte[]> CreateAsync(string cacheKey, string verifyCode, int width, int height);

        Task<bool> ValidateAsync(string cacheKey, string verifyCode);
    }
}