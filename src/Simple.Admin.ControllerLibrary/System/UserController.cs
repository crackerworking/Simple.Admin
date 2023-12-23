using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Permission;
using Simple.Admin.Application.Contracts.System.Models.User;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.ControllerLibrary.System
{
    [Authorize]
    public class UserController : MiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IPermissionService permissionService, IRoleService roleService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _roleService = roleService;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Query")]
        public async Task<MessageModel> GetUserList([FromBody] UserSearch search)
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
        public async Task<MessageModel> AddUser([FromBody] UserPlus input)
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
        public async Task<MessageModel> RemoveUser([FromBody] PrimaryKey input)
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
        public async Task<MessageModel> UpdatePassword([FromBody] PrimaryKey input)
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
        public async Task<MessageModel> SetUserRole([FromBody] SetUserRoleIn input)
            => await _permissionService.SetUserRoleAsync(input);

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:User:Passed")]
        public async Task<MessageModel> SwitchState([FromBody] PrimaryKey input)
            => await _userService.SwitchStateAsync(input);

        /// <summary>
        /// 角色选项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<MessageModel<IList<Option>>> GetRoleOptions() => _roleService.GetRoleOptions();
    }
}