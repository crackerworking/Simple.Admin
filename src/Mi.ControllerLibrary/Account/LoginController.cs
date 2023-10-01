using Mi.Application.Contracts.System;
using Mi.Domain.Shared.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mi.ControllerLibrary.Account
{
    [Route("/api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogService _logService;
        private readonly HttpContext _httpContext;

        public LoginController(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor
            , ILogService logService)
        {
            _permissionService = permissionService;
            _logService = logService;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        [HttpPost]
        public async Task<ResponseStructure> Do(string userName, string password, string code)
        {
            var result = await _permissionService.LoginAsync(userName, password, code);
            await _logService.WriteLoginLogAsync(userName, result.Code == response_type.Success, result.Message ?? "");
            return result;
        }

        [HttpPost]
        public async Task<ResponseStructure> New(string userName, string password)
            => await _permissionService.RegisterAsync(userName, password);

        public async Task<IActionResult> Exit()
        {
            await _permissionService.LogoutAsync();
            return Redirect("/Account/Login");
        }
    }
}