using System.ComponentModel.DataAnnotations;

using Mi.Application.Contracts.Cache;

namespace Mi.ControllerLibrary.Development
{
    [ApiRoute]
    [Authorize]
    public class CacheKeyManagerController : ControllerBase
    {
        private readonly ICacheKeyManagerService _keyService;

        public CacheKeyManagerController(ICacheKeyManagerService keyService)
        {
            _keyService = keyService;
        }

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Query")]
        public async Task<ResponseStructure<IList<Option>>> GetAllKeys(string? vague, int cacheType) => await _keyService.GetAllKeysAsync(vague);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Remove")]
        public async Task<ResponseStructure> RemoveKey([Required(ErrorMessage = "key不能为空")] string key) => await _keyService.RemoveKeyAsync(key);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:GetData")]
        public async Task<ResponseStructure<string>> GetData([Required(ErrorMessage = "key不能为空")] string key) => await _keyService.GetDataAsync(key);
    }
}