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
        private readonly IDictService _dictService;

        public PublicService(IOptionsMonitor<PaConfigModel> uiConfig, IDictService dictService
            , ICurrentUser miUser, IRepository<SysMessage> messageRepo, ICaptcha captcha)
        {
            _dictService = dictService;
            _uiConfig = uiConfig.CurrentValue;
            _miUser = miUser;
            _messageRepo = messageRepo;
            _captcha = captcha;
        }

        public async Task<bool> WriteMessageAsync(string title, string content, IList<long> receiveUsers)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) throw new FriendlyException("消息的标题和内容不能为空");
            if (receiveUsers == null || receiveUsers.Count == 0) throw new FriendlyException("接收消息用户不能为空");

            var now = DateTime.Now;
            var list = receiveUsers.Select(x => new SysMessage { Title = title, Content = content, Readed = 0, CreatedBy = _miUser.UserId, CreatedOn = now, ReceiveUser = x }).ToList();
            await _messageRepo.AddRangeAsync(list);

            return true;
        }

        public async Task<PaConfigModel> ReadConfigAsync()
        {
            var config = await _dictService.GetAsync<SysConfigModel>(DictKeyConst.UiConfig);
            var result = _uiConfig;
            result.logo.title = config.header_name;
            result.logo.image = config.logo ?? "";
            result.tab.index.title = config.home_page_name;
            result.tab.index.href = config.home_page_url;
            return result;
        }

        public async Task<ResponseStructure> SetUiConfigAsync(SysConfigModel operation)
        {
            await _dictService.SetAsync(operation);
            return ResponseHelper.Success();
        }

        public async Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync()
        {
            var config = await _dictService.GetAsync<SysConfigModel>(DictKeyConst.UiConfig);
            return ResponseHelper.Success("查询成功").As(config);
        }

        public ResponseStructure HasPermission(string authCode)
        {
            var flag = _miUser.AuthCodes.Contains(authCode);
            return flag ? ResponseHelper.Success("有") : ResponseHelper.Fail("无");
        }

        public Task<byte[]> LoginCaptchaAsync()
        {
            string v = StringHelper.GetMacAddress();
            return _captcha.CreateAsync(v, StringHelper.GetRandomString(5), 160, 30);
        }
    }
}