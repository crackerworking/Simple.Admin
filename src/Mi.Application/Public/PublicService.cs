using Mi.Core.Abnormal;
using Mi.Core.API;
using Mi.Core.GlobalVar;
using Mi.Core.Helper;
using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.IRepository.BASE;
using Mi.IService.Public;

using Microsoft.Extensions.Options;

namespace Mi.Application.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly PaConfigModel _uiConfig;
        private readonly IMiUser _miUser;
        private readonly ResponseStructure _msg;
        private readonly IDictService _dictService;

        public PublicService(IOptionsMonitor<PaConfigModel> uiConfig, IDictService dictService
            , IMiUser miUser
            , ResponseStructure msg)
        {
            _dictService = dictService;
            _uiConfig = uiConfig.CurrentValue;
            _miUser = miUser;
            _msg = msg;
        }

        public async Task<bool> WriteMessageAsync(string title, string content, IList<long> receiveUsers)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) throw new FriendlyException("消息的标题和内容不能为空");
            if (receiveUsers == null || receiveUsers.Count == 0) throw new FriendlyException("接收消息用户不能为空");

            var repo = DotNetService.Get<IRepositoryBase<SysMessage>>();
            var now = TimeHelper.LocalTime();
            var list = receiveUsers.Select(x => new SysMessage { Title = title, Content = content, Readed = 0, CreatedBy = _miUser.UserId, CreatedOn = now, ReceiveUser = x }).ToList();
            await repo.AddManyAsync(list);

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
            return _msg.Success();
        }

        public async Task<ResponseStructure<SysConfigModel>> GetUiConfigAsync()
        {
            var config = await _dictService.GetAsync<SysConfigModel>(DictKeyConst.UiConfig);
            return _msg.Success("查询成功").As(config);
        }

        public ResponseStructure HasPermission(string authCode)
        {
            var flag = _miUser.AuthCodes.Contains(authCode);
            return flag ? _msg.Success("有") : _msg.Fail("无");
        }
    }
}