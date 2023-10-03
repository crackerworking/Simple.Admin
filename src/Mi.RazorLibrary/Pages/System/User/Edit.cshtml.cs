using Mi.Application.Contracts.System.Models.User;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.User
{
    internal class EditModel : PageModel
    {
        public SysUserFull UserInfo { get; set; }
    }
}