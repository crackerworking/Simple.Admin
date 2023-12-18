using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Models.UI;

namespace Simple.Admin.ControllerLibrary.System
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
        public async Task<MessageModel> SetUiConfig([FromBody] Dictionary<string, string> operation)
        {
            await _dictionaryApi.SetAsync(operation);
            return new MessageModel(response_type.Success, "success");
        }

        /// <summary>
        /// 获取UI配置
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<MessageModel<SysConfigModel>> GetUiConfig() => await _uiConfigService.GetUiConfigAsync();
    }
}