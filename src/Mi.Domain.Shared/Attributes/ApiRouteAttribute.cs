using Microsoft.AspNetCore.Mvc;

namespace Mi.Domain.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiRouteAttribute : RouteAttribute
    {
        public ApiRouteAttribute() : base("/api/[controller]/[action]")
        {
        }
    }
}