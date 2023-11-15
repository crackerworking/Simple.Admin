using Microsoft.AspNetCore.Mvc;

namespace Mi.Domain.Shared.Attributes
{
    /// <summary>
    /// 接口路由
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiRouteAttribute : RouteAttribute
    {
        public ApiRouteAttribute() : base("/api/[controller]/[action]")
        {
        }
    }
}