using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models;

using Microsoft.AspNetCore.Authorization;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
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
        public async Task<ResponseStructure> AddRole(string name, string? remark)
            => await _roleService.AddRoleAsync(name, remark);

        [HttpPost, AuthorizeCode("System:Role:Remove")]
        public async Task<ResponseStructure> RemoveRole(long id)
             => await _roleService.RemoveRoleAsync(id);

        [HttpPost, AuthorizeCode("System:Role:Query")]
        public async Task<ResponseStructure> GetRoleList([FromBody] RoleSearch search)
            => await _roleService.GetRoleListAsync(search);

        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<ResponseStructure> UpdateRole(long id, string name, string remark)
            => await _roleService.UpdateRoleAsync(id, name, remark);

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure> SetRoleFunctions([FromForm] long id, [FromForm] IList<long> funcIds)
        {
            return await _permissionService.SetRoleFunctionsAsync(id, funcIds);
        }

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure<IList<long>>> GetRoleFunctionIds(long id)
            => await _permissionService.GetRoleFunctionIdsAsync(id);
    }
}