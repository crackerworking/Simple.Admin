using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.Shared.Core
{
    public abstract class Startup
    {
        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app);
    }
}