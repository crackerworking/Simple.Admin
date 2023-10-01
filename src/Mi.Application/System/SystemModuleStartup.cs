using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Application.System
{
    internal class SystemModuleStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
        }

        public override void ConfigureService(IServiceCollection services)
        {
            services.AddAutoMapper(conf =>
            {
            });
        }
    }
}