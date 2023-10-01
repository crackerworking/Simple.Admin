using Mi.Application.Contracts.Public;
using Mi.Application.Contracts.System;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class UiConfigController : ControllerBase
    {
        private readonly IPublicService _publicService;
        private readonly IDictService _dictService;

        public UiConfigController(IPublicService publicService, IDictService dictService)
        {
            _publicService = publicService;
            _dictService = dictService;
        }

        [HttpPost, AuthorizeCode("System:SetConfig")]
        public async Task<ResponseStructure> SetUiConfig([FromBody] Dictionary<string, string> operation) => await _dictService.SetAsync(operation);

        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<ResponseStructure<SysConfigModel>> GetUiConfig() => await _publicService.GetUiConfigAsync();
    }
}