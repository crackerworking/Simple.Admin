using System.ComponentModel.DataAnnotations;

using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.User;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [HttpPost]
        [AuthorizeCode("System:User:Query")]
        public async Task<ResponseStructure> GetUserList([FromBody] UserSearch search)
        {
            throw new Exception("测试");
            return await _userService.GetUserListAsync(search);
        }

        [HttpPost]
        [AuthorizeCode("System:User:Add")]
        public async Task<ResponseStructure> AddUser([Required(ErrorMessage = "用户名不能为空")] string userName)
        {
            return await _userService.AddUserAsync(userName);
        }

        [HttpPost]
        [AuthorizeCode("System:User:Remove")]
        public async Task<ResponseStructure> RemoveUser(long userId)
        {
            return await _userService.RemoveUserAsync(userId);
        }

        [HttpPost]
        [AuthorizeCode("System:User:UpdatePassword")]
        public async Task<ResponseStructure> UpdatePassword(long userId)
        {
            return await _userService.UpdatePasswordAsync(userId);
        }

        [HttpPost]
        [AuthorizeCode("System:User:SetUserRole")]
        public async Task<ResponseStructure> SetUserRole(long userId, List<long> roleIds)
            => await _permissionService.SetUserRoleAsync(userId, roleIds);

        [HttpPost]
        [AuthorizeCode("System:User:Passed")]
        public async Task<ResponseStructure> PassedUser(long id)
            => await _userService.PassedUserAsync(id);
    }
}