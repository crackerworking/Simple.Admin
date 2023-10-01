using Mi.DataDriver.Dapper;
using Mi.DataDriver.EntityFrameworkCore;
using Mi.Domain.DataAccess;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.DataDriver
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