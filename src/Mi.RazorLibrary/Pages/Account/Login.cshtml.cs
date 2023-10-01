using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.Account
{
    public class LoginModel : PageModel
    {
        public string FooterWord { get; set; }

        public void OnGet()
        {
        }
    }
}