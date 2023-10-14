using Mi.Application.Contracts.Public;
using Mi.Domain.Shared.Core;

namespace Mi.Application.Public
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

        public async Task<ResponseStructure> SetUiConfigAsync(SysConfigModel operation)
        {
            var dict = new Dictionary<string, string>();
            foreach (var prop in typeof(SysConfigModel).GetProperties())
            {
                dict.TryAdd(prop.Name, (string?)prop.GetValue(operation) ?? "");
            }
            await _dictionaryApi.SetAsync(dict);
            return Back.Success();
        }

        public async Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync()
        {
            var config = await _dictionaryApi.GetManyAsync<SysConfigModel>(DictKeyConst.UiConfig);
            return Back.Success("查询成功").As(config);
        }

        public ResponseStructure HasPermission(string authCode)
        {
            var flag = _miUser.AuthCodes.Contains(authCode);
            return flag ? Back.Success("有") : Back.Fail("无");
        }

        public Task<byte[]> LoginCaptchaAsync(Guid guid)
        {
            return _captcha.CreateAsync(guid.ToString(), StringHelper.GetRandomString(5), 120, 30);
        }
    }
}