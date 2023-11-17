using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Helper;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.GlobalVars;
using Simple.Admin.Domain.Shared.Models.UI;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.System.Impl
{
    internal class UIConfigService : IUIConfigService
    {
        private readonly IQuickDict _quickDict;

        public UIConfigService(IQuickDict quickDict)
        {
            _quickDict = quickDict;
        }

        public async Task<MessageModel> SetUiConfigAsync(SysConfigModel operation)
        {
            var dict = new Dictionary<string, string>();
            foreach (var prop in typeof(SysConfigModel).GetProperties())
            {
                dict.TryAdd(prop.Name, (string?)prop.GetValue(operation) ?? "");
            }
            await _quickDict.SetAsync(dict);
            return Back.Success();
        }

        public async Task<MessageModel<SysConfigModel>> GetUiConfigAsync()
        {
            var config = await _quickDict.GetManyAsync<SysConfigModel>(DictKeyConst.UiConfig);
            return Back.Success("查询成功").As(config);
        }
    }
}