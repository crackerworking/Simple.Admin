namespace Mi.Application.Contracts.Cache
{
    public interface ICacheKeyManagerService
    {
        Task<ResponseStructure<IList<Option>>> GetAllKeysAsync(string? vague, int cacheType = 1);

        Task<ResponseStructure> RemoveKeyAsync(string key);

        Task<ResponseStructure<string>> GetDataAsync(string key);
    }
}
