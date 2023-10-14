using Mi.Application.Contracts.Cache;
using Mi.Application.Contracts.Cache.Models;

namespace Mi.ControllerLibrary.Development
{
    [ApiRoute]
    [AllowAnonymous]
    public class CacheKeyManagerController : ControllerBase
    {
        private readonly ICacheKeyManagerService _keyService;

        public CacheKeyManagerController(ICacheKeyManagerService keyService)
        {
            _keyService = keyService;
        }

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Query")]
        public async Task<ResponseStructure<IList<Option>>> GetAllKeys([FromBody] CacheKeySearch input) => await _keyService.GetAllKeysAsync(input);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Remove")]
        public async Task<ResponseStructure> RemoveKey([FromBody] CacheKeyIn input) => await _keyService.RemoveKeyAsync(input);

        [HttpPost]
        [AuthorizeCode("Development:CacheKey:GetData")]
        public async Task<ResponseStructure<string>> GetData([FromBody] CacheKeyIn input) => await _keyService.GetDataAsync(input);
    }
}