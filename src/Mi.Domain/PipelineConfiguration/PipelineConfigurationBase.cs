using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public abstract class PipelineConfigurationBase
    {
        public abstract void ConfigureService(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app);
    }
}