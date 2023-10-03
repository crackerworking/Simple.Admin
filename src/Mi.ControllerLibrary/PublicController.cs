using Mi.Application.Contracts.Public;
using Mi.Domain.Shared.Models.UI;

namespace Mi.ControllerLibrary
{
    [ApiRoute]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService _publicService;

        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }

        [HttpGet]
        public async Task<FileResult> LoginCaptcha(Guid guid)
        {
            var bytes = await _publicService.LoginCaptchaAsync(guid);
            return File(bytes, "image/png");
        }

        [HttpGet]
        public async Task<PaConfigModel> Config()
        {
            return await _publicService.ReadConfigAsync();
        }

        [HttpPost]
        public ResponseStructure HasPermission(string authCode) => _publicService.HasPermission(authCode);
    }
}