using System.Data;

using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Permission;
using Mi.Application.Contracts.System.Models.Role;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<ResponseStructure> AddRole([FromBody] RolePlus input)
            => await _roleService.AddRoleAsync(input);

        [HttpPost, AuthorizeCode("System:Role:Remove")]
        public async Task<ResponseStructure> RemoveRole([FromBody] PrimaryKey input)
             => await _roleService.RemoveRoleAsync(input);

        [HttpPost, AuthorizeCode("System:Role:Query")]
        public async Task<ResponseStructure> GetRoleList([FromBody] RoleSearch search)
            => await _roleService.GetRoleListAsync(search);

        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<ResponseStructure> UpdateRole([FromBody] RoleEdit input)
            => await _roleService.UpdateRoleAsync(input);

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure> SetRoleFunctions([FromBody] SetRoleFunctionsIn input)
        {
            return await _permissionService.SetRoleFunctionsAsync(input);
        }

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure<IList<long>>> GetRoleFunctionIds([FromBody] PrimaryKey input)
            => await _permissionService.GetRoleFunctionIdsAsync(input);
    }
}