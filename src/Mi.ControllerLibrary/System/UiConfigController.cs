using Mi.Application.Contracts.Public;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class UiConfigController : ControllerBase
    {
        private readonly IPublicService _publicService;
        private readonly IDictionaryApi _dictionaryApi;

        public UiConfigController(IPublicService publicService, IDictionaryApi dictionaryApi)
        {
            _publicService = publicService;
            _dictionaryApi = dictionaryApi;
        }

        [HttpPost, AuthorizeCode("System:SetConfig")]
        public async Task<ResponseStructure> SetUiConfig([FromBody] Dictionary<string, string> operation)
        {
            await _dictionaryApi.SetAsync(operation);
            return new ResponseStructure(response_type.Success, "success");
        }

        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<ResponseStructure<SysConfigModel>> GetUiConfig() => await _publicService.GetUiConfigAsync();
    }
}