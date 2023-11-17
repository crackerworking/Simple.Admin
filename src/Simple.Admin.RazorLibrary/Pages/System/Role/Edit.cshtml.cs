using Microsoft.AspNetCore.Mvc.RazorPages;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Role;

namespace Simple.Admin.RazorLibrary.Pages.System.Role
{
    public class EditModel : PageModel
    {
        private readonly IRoleService _roleService;

        public SysRoleFull Role { get; set; } = new();

        public EditModel(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task OnGetAsync(long? id)
        {
            var r1 = await _roleService.GetRoleAsync(id.GetValueOrDefault());
            if (r1.IsOk())
            {
                Role = r1.Result!;
            }
        }
    }
}