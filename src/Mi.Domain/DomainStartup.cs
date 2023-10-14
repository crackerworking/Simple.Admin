using Mi.Domain.Services;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain
{
    internal class DomainStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageFactory>(MessageFactory.Instance);
        }
    }
}