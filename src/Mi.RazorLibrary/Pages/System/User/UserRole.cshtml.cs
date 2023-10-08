using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Result;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.User
{
    public class UserRoleModel : PageModel
    {
        private readonly IPermissionService _permissionService;

        public List<UserRoleOption> Options { get; set; } = new();

        public long Id { get; set; }

        public UserRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnGetAsync(long? id)
        {
            Id = id.GetValueOrDefault();
            var r1 = await _permissionService.GetUserRolesAsync(Id);
            if (r1.Ok())
            {
                Options = r1.Result!.ToList();
            }
        }
    }
}