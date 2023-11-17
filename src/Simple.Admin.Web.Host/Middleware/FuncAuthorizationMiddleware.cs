using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Attributes;
using Simple.Admin.Domain.Shared.GlobalVars;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Web.Host.Middleware
{
    public class FuncAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly ILogger<FuncAuthorizationMiddleware> _logger;
        private readonly string[] IGNORE_PAGES = { "/entry" };
        private readonly List<Type> CONTROLLER_TYPES = new List<Type> { typeof(Controller), typeof(ControllerBase) };
        private readonly List<string?> VIEW_TYPES = new List<string?> { typeof(IActionResult).FullName, typeof(ViewResult).FullName, typeof(RedirectResult).FullName };

        public FuncAuthorizationMiddleware(ILogger<FuncAuthorizationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var userModel = context.GetUser();
            var pageRoute = (string?)context.Request.RouteValues["page"] ?? "";

            if (!IGNORE_PAGES.Contains(pageRoute.ToLower()) && userModel != null && userModel.UserId > 0)
            {
                var path = context.Request.Path.Value;
                if (!userModel.IsSuperAdmin)
                {
                    var flag = false;
                    if (userModel.PowerItems != null)
                    {
                        var endpoint = context.GetEndpoint();
                        var attr = endpoint?.Metadata.GetMetadata<AuthorizeCodeAttribute>();
                        if (endpoint != null && attr != null)
                        {
                            flag = userModel.PowerItems!.Any(x => x.AuthCode == attr.Code);
                        }
                    }
                    if (!flag)
                    {
                        _logger.LogWarning($"'用户Id：{userModel.UserId}，用户名：{userModel.UserName}'访问地址`{path}`权限不足");

                        if (!pageRoute.IsNull())
                        {
                            context.Response.Redirect("/html/403.html");
                            return;
                        }

                        await context.Response.WriteAsJsonAsync(new MessageModel(response_type.Forbidden, "权限不足，无法访问或操作"));
                        return;
                    }
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

        private List<Type> GetControllerTypesCache()
        {
            var cache = App.Provider.GetRequiredService<IMemoryCache>();
            var types = cache.Get<List<Type>>(CacheConst.CONTROLLER_TYPES);
            if (types == null)
            {
                var assembly = Assembly.Load("Simple.Admin.Admin");
                types = assembly?.GetTypes().Where(x => x.BaseType != null && CONTROLLER_TYPES.Contains(x.BaseType)).ToList() ?? new List<Type>();
                cache.Set(CacheConst.CONTROLLER_TYPES, types, CacheConst.Year);
            }
            return types;
        }
    }
}