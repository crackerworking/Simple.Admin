using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Permission;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.ControllerLibrary.Account
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
            await _logService.WriteLoginLogAsync(input.username, result.Code == response_type.Success, result.Message ?? "");
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
    }
}