using Mi.Application.Contracts.System;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class UiConfigController : MiControllerBase
    {
        private readonly IQuickDict _dictionaryApi;
        private readonly IUIConfigService _uiConfigService;

        public UiConfigController(IQuickDict dictionaryApi, IUIConfigService uiConfigService)
        {
            _dictionaryApi = dictionaryApi;
            _uiConfigService = uiConfigService;
        }

        /// <summary>
        /// 更新UI配置
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:SetConfig")]
        public async Task<ResponseStructure> SetUiConfig([FromBody] Dictionary<string, string> operation)
        {
            await _dictionaryApi.SetAsync(operation);
            return new ResponseStructure(response_type.Success, "success");
        }

        /// <summary>
        /// 获取UI配置
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<ResponseStructure<SysConfigModel>> GetUiConfig() => await _uiConfigService.GetUiConfigAsync();
    }
}