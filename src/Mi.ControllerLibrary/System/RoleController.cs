﻿using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Permission;
using Mi.Application.Contracts.System.Models.Role;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class RoleController : MiControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<ResponseStructure> AddRole([FromBody] RolePlus input)
            => await _roleService.AddRoleAsync(input);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Role:Remove")]
        public async Task<ResponseStructure> RemoveRole([FromBody] PrimaryKey input)
             => await _roleService.RemoveRoleAsync(input);

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Role:Query")]
        public async Task<ResponseStructure> GetRoleList([FromBody] RoleSearch search)
            => await _roleService.GetRoleListAsync(search);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<ResponseStructure> UpdateRole([FromBody] RoleEdit input)
            => await _roleService.UpdateRoleAsync(input);

        /// <summary>
        /// 设置角色功能
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure> SetRoleFunctions([FromBody] SetRoleFunctionsIn input)
        {
            return await _permissionService.SetRoleFunctionsAsync(input);
        }

        /// <summary>
        /// 获取角色功能
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<ResponseStructure<IList<long>>> GetRoleFunctionIds([FromBody] PrimaryKey input)
            => await _permissionService.GetRoleFunctionIdsAsync(input);
    }
}