using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.DataDriver.Dapper;
using Simple.Admin.DataDriver.EntityFrameworkCore;
using Simple.Admin.Domain.DataAccess;

namespace Simple.Admin.DataDriver
{
    public static class RepositorySetup
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDapperRepository, DapperRepository>();
        }
    }
}