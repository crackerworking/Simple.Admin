using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Shared;

namespace Simple.Admin.Web.Host.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isLogin = context.User.Identity?.IsAuthenticated ?? false;
            var data = context.GetUserData();
            if (isLogin && !string.IsNullOrWhiteSpace(data))
            {
                using var scoped = App.Provider.CreateScope();
                var permissionService = scoped.ServiceProvider.GetService<IPermissionService>()!;
                var userModel = await permissionService.QueryUserModelCacheAsync(data);
                context.Features.Set(userModel);
            }

            await _next(context);
        }
    }
}