using Simple.Admin.Application.Contracts.Public;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly ICurrentUser _miUser;
        private readonly IRepository<SysMessage> _messageRepo;
        private readonly ICaptcha _captcha;
        private readonly IQuickDict _dictionaryApi;
        private readonly IDictService _dictService;

        public PublicService(IDictService dictService
            , ICurrentUser miUser, IRepository<SysMessage> messageRepo, ICaptcha captcha, IQuickDict dictionaryApi)
        {
            _dictService = dictService;
            _miUser = miUser;
            _messageRepo = messageRepo;
            _captcha = captcha;
            _dictionaryApi = dictionaryApi;
        }

        public Task<byte[]> LoginCaptchaAsync(Guid guid)
        {
            var code = StringHelper.GetRandomString(5);
            return _captcha.CreateAsync(guid.ToString(), code, 120, 30);
        }
    }
}