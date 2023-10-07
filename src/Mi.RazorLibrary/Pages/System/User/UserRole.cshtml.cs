using Mi.Application.Contracts.System.Models.Result;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.User
{
    public class UserRoleModel : PageModel
    {
        public List<UserRoleOption> Options { get; set; }
    }
}