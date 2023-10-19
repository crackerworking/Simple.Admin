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

        [HttpPost]
        public async Task<ResponseStructure> Do([FromBody] LoginIn input)
        {
            var result = await _permissionService.LoginAsync(input);
            await _logService.WriteLoginLogAsync(input.userName, result.Code == response_type.Success, result.Message ?? "");
            return result;
        }

        [HttpPost]
        public async Task<ResponseStructure> New([FromBody] RegisterIn input)
            => await _permissionService.RegisterAsync(input);

        public async Task<IActionResult> Exit()
        {
            await _permissionService.LogoutAsync();
            return Redirect("/login");
        }
    }
}