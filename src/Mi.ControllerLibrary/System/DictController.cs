using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models;
using Mi.Application.Contracts.System.Models.Result;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class DictController : ControllerBase
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            _dictService = dictService;
        }

        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<ResponseStructure<PagingModel<DictItem>>> GetDictList([FromBody] DictSearch search)
            => await _dictService.GetDictListAsync(search);

        [HttpPost, AuthorizeCode("System:Dict:AddOrUpdate")]
        public async Task<ResponseStructure> AddOrUpdateDict([FromBody] DictOperation operation)
            => await _dictService.AddOrUpdateDictAsync(operation);

        [HttpPost, AuthorizeCode("System:Dict:Remove")]
        public async Task<ResponseStructure> RemoveDict(IList<string> ids)
            => await _dictService.RemoveDictAsync(ids);

        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<List<Option>> GetParentList()
            => await _dictService.GetParentListAsync();
    }
}