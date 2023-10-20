using System.ComponentModel.DataAnnotations;

using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Permission;
using Mi.Application.Contracts.System.Models.User;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class UserController : MiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Query")]
        public async Task<ResponseStructure> GetUserList([FromBody] UserSearch search)
        {
            return await _userService.GetUserListAsync(search);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Add")]
        public async Task<ResponseStructure> AddUser([FromBody] UserPlus input)
        {
            return await _userService.AddUserAsync(input);
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Remove")]
        public async Task<ResponseStructure> RemoveUser([FromBody] PrimaryKey input)
        {
            return await _userService.RemoveUserAsync(input);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns>一个随机密码</returns>
        [HttpPost]
        [AuthorizeCode("System:User:UpdatePassword")]
        public async Task<ResponseStructure> UpdatePassword([FromBody] PrimaryKey input)
        {
            return await _userService.UpdatePasswordAsync(input);
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:SetUserRole")]
        public async Task<ResponseStructure> SetUserRole([FromBody] SetUserRoleIn input)
            => await _permissionService.SetUserRoleAsync(input);

        /// <summary>
        /// 允许登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Passed")]
        public async Task<ResponseStructure> PassedUser([FromBody] PrimaryKey input)
            => await _userService.PassedUserAsync(input);
    }
}