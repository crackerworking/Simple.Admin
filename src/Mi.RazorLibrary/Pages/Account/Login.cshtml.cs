using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Mi.RazorLibrary.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IQuickDict _quickDict;

        public Guid GuidKey { get; private set; }

        public string FooterWord { get; set; }

        public LoginModel(IConfiguration configuration, IQuickDict quickDict)
        {
            _configuration = configuration;
            _quickDict = quickDict;
        }

        public bool ShowLoginCaptcha => Convert.ToBoolean(_configuration["ShowLoginCaptcha"]);

        public void OnGet()
        {
            GuidKey = Guid.NewGuid();
            FooterWord = _quickDict["login_footer_word"];
        }
    }
}