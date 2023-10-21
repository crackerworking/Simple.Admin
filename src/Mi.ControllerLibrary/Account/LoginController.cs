using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Permission;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.Account
{
    public class LoginController : MiControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogService _logService;

        public LoginController(IPermissionService permissionService
            , ILogService logService)
        {
            _permissionService = permissionService;
            _logService = logService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Do([FromBody] LoginIn input)
        {
            var result = await _permissionService.LoginAsync(input);
            await _logService.WriteLoginLogAsync(input.userName, result.Code == response_type.Success, result.Message ?? "");
            return result;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> New([FromBody] RegisterIn input)
            => await _permissionService.RegisterAsync(input);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Exit()
        {
            await _permissionService.LogoutAsync();
            return Redirect("/login");
        }
    }
}