using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.User
{
    public static class CurrentUserSetup
    {
        public static void AddCurrentUser(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUser>(sp =>
            {
                IHttpContextAccessor httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    return httpContext.Features.Get<CurrentUser>() ?? new CurrentUser();
                }
                return new CurrentUser();
            });
        }
    }
}