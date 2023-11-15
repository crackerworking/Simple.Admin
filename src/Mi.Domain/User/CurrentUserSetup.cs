using Mi.Domain.Extension;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.User
{
    public static class CurrentUserSetup
    {
        public static void AddCurrentUser(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser>(sp =>
            {
                IHttpContextAccessor httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;
                var result = new CurrentUser();
                if (httpContext != null)
                {
                    var user = httpContext.GetUser();
                    if (user.PowerItems != null && user != null)
                    {
                        result.FuncIds = user.PowerItems.Select(x => x.Id).ToList();
                        result.AuthCodes = user.PowerItems.Select(x => x.AuthCode).Where(x => !string.IsNullOrEmpty(x)).ToList();
                        result.UserId = user.UserId;
                        result.UserName = user.UserName;
                        result.IsSuperAdmin = user.IsSuperAdmin;
                    }
                }
                return result;
            });
        }
    }
}