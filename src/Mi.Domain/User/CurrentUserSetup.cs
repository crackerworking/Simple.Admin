using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.User
{
    public static class CurrentUserSetup
    {
        public static void AddCurrentUser(this IServiceCollection services)
        {
            //TODO:
            services.AddSingleton<ICurrentUser>(sp =>
            {
                return new CurrentUser();
            });
        }
    }
}