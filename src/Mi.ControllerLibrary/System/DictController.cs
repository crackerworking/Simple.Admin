using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Dict;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class DictController : MiControllerBase
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            _dictService = dictService;
        }

        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<ResponseStructure<PagingModel<DictItem>>> GetDictList([FromBody] DictSearch search)
            => await _dictService.GetDictListAsync(search);

        [HttpPost, AuthorizeCode("System:Dict:Add")]
        public async Task<ResponseStructure> AddAsync([FromBody] DictPlus operation)
            => await _dictService.AddAsync(operation);

        [HttpPost, AuthorizeCode("System:Dict:Update")]
        public async Task<ResponseStructure> UpdateAsync([FromBody] DictEdit operation)
            => await _dictService.UpdateAsync(operation);

        [HttpPost, AuthorizeCode("System:Dict:Remove")]
        public async Task<ResponseStructure> RemoveDict([FromBody] PrimaryKeys input)
            => await _dictService.RemoveDictAsync(input);

        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<List<Option>> GetParentList()
            => await _dictService.GetParentListAsync();
    }
}