using Microsoft.AspNetCore.Mvc.RazorPages;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.User;

namespace Simple.Admin.RazorLibrary.Pages.System.User
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public SysUserFull UserInfo { get; set; } = new();

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync(long? id)
        {
            var r1 = await _userService.GetUserAsync(id.GetValueOrDefault());
            if (r1.IsOk())
            {
                UserInfo = r1.Result!;
            }
        }
    }
}