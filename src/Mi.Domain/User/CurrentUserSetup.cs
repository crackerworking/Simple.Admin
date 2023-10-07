using Mi.Domain.Extension;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models;

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
                    var user = httpContext.Features.Get<UserModel>() ?? new UserModel();
                    var user2 = httpContext.GetUser();
                    if (user.PowerItems != null && user2 != null)
                    {
                        result.FuncIds = user.PowerItems.Select(x => x.Id).ToList();
                        result.AuthCodes = user.PowerItems.Select(x => x.AuthCode).Where(x => !string.IsNullOrEmpty(x)).ToList();
                        result.UserId = user2.UserId;
                        result.UserName = user2.UserName;
                        result.IsSuperAdmin = user2.IsSuperAdmin;
                    }
                }
                return result;
            });
        }
    }
}