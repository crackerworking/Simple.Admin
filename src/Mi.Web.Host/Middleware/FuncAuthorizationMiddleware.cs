using Mi.Domain.Shared.Attributes;
using Mi.Domain.Shared.GlobalVars;
using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Mi.Domain.Extension;
using Mi.Domain.PipelineConfiguration;
using Microsoft.Extensions.Caching.Memory;
using Mi.Domain.Shared.Response;

namespace Mi.Web.Host.Middleware
{
    public class FuncAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly ILogger<FuncAuthorizationMiddleware> _logger;
        private readonly string[] IGNORE_CONTROLLERS = { "home" };
        private readonly List<Type> CONTROLLER_TYPES = new List<Type> { typeof(Controller), typeof(ControllerBase) };
        private readonly List<string?> VIEW_TYPES = new List<string?> { typeof(IActionResult).FullName, typeof(ViewResult).FullName, typeof(RedirectResult).FullName };

        public FuncAuthorizationMiddleware(ILogger<FuncAuthorizationMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var userModel = context.GetUser();
            var controllerName = (string?)context.Request.RouteValues["controller"] ?? "";
            var actionName = (string?)context.GetRouteValue("action") ?? "";

            if (!IGNORE_CONTROLLERS.Contains(controllerName.ToLower()) && userModel != null && userModel.UserId > 0)
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

                        var types = GetControllerTypesCache();
                        foreach (var item in types)
                        {
                            if (item.Name.Equals($"{controllerName}Controller", StringComparison.CurrentCultureIgnoreCase) || item.Name.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var method = item.GetMethods().Where(x => x.Name.ToLower() == actionName.ToLower()).FirstOrDefault();
                                var returnType = method?.ReturnType;
                                if (returnType != null && VIEW_TYPES.Contains(returnType.FullName))
                                {
                                    context.Response.Redirect("/html/403.html");
                                    return;
                                }
                            }
                        }

                        await context.Response.WriteAsJsonAsync(new ResponseStructure(response_type.Forbidden, "权限不足，无法访问或操作"));
                        return;
                    }
                }
            }
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

        private List<Type> GetControllerTypesCache()
        {
            var cache = ServiceManager.Get<IMemoryCache>();
            var types = cache.Get<List<Type>>(CacheConst.CONTROLLER_TYPES);
            if (types == null)
            {
                var assembly = Assembly.Load("Mi.Admin");
                types = assembly?.GetTypes().Where(x => x.BaseType != null && CONTROLLER_TYPES.Contains(x.BaseType)).ToList() ?? new List<Type>();
                cache.Set(CacheConst.CONTROLLER_TYPES, types, CacheConst.Year);
            }
            return types;
        }
    }
}
