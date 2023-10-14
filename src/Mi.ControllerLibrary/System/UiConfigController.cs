﻿using Mi.Application.Contracts.System;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class UiConfigController : ControllerBase
    {
        private readonly IQuickDict _dictionaryApi;
        private readonly IUIConfigService _uiConfigService;

        public UiConfigController(IQuickDict dictionaryApi, IUIConfigService uiConfigService)
        {
            _dictionaryApi = dictionaryApi;
            _uiConfigService = uiConfigService;
        }

        [HttpPost, AuthorizeCode("System:SetConfig")]
        public async Task<ResponseStructure> SetUiConfig([FromBody] Dictionary<string, string> operation)
        {
            await _dictionaryApi.SetAsync(operation);
            return new ResponseStructure(response_type.Success, "success");
        }

        [HttpPost, AuthorizeCode("System:GetConfig")]
        public async Task<ResponseStructure<SysConfigModel>> GetUiConfig() => await _uiConfigService.GetUiConfigAsync();
    }
}