using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Result;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.User
{
    public class UserRoleModel : PageModel
    {
        private readonly IPermissionService _permissionService;

        public List<UserRoleOption> Options { get; set; } = new();

        public UserRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnGetAsync(long? id)
        {
            var r1 = await _permissionService.GetUserRolesAsync(id.GetValueOrDefault());
            if (r1.Ok())
            {
                Options = r1.Result!.ToList();
            }
        }
    }
}