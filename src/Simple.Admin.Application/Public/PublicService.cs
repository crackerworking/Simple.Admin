using Simple.Admin.Application.Contracts.Public;
using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Domain.DataAccess;
using Simple.Admin.Domain.Entities.System;
using Simple.Admin.Domain.Helper;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.GlobalVars;
using Simple.Admin.Domain.Shared.Models.UI;

namespace Simple.Admin.Application.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly PaConfigModel _uiConfig;
        private readonly ICurrentUser _miUser;
        private readonly IRepository<SysMessage> _messageRepo;
        private readonly ICaptcha _captcha;
        private readonly IQuickDict _dictionaryApi;
        private readonly IDictService _dictService;

        public PublicService(IOptionsMonitor<PaConfigModel> uiConfig, IDictService dictService
            , ICurrentUser miUser, IRepository<SysMessage> messageRepo, ICaptcha captcha, IQuickDict dictionaryApi)
        {
            _dictService = dictService;
            _uiConfig = uiConfig.CurrentValue;
            _miUser = miUser;
            _messageRepo = messageRepo;
            _captcha = captcha;
            _dictionaryApi = dictionaryApi;
        }

        public async Task<PaConfigModel> ReadConfigAsync()
        {
            var config = await _dictionaryApi.GetManyAsync<SysConfigModel>(DictKeyConst.UiConfig);
            var result = _uiConfig;
            result.logo.title = config.header_name;
            result.logo.image = config.logo ?? "";
            result.tab.index.title = config.home_page_name;
            result.tab.index.href = config.home_page_url;
            return result;
        }

        public Task<byte[]> LoginCaptchaAsync(Guid guid)
        {
            var code = StringHelper.GetRandomString(5);
            return _captcha.CreateAsync(guid.ToString(), code, 120, 30);
        }
    }
}