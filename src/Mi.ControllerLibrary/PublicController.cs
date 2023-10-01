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

        [HttpGet("captcha")]
        public async Task<FileResult> LoginCaptcha()
        {
            var bytes = await _publicService.LoginCaptchaAsync();
            return File(bytes, "image/png");
        }

        [HttpGet("config")]
        public async Task<PaConfigModel> Config()
        {
            return await _publicService.ReadConfigAsync();
        }

        [HttpPost("has-permission")]
        public ResponseStructure HasPermission(string authCode) => _publicService.HasPermission(authCode);
    }
}