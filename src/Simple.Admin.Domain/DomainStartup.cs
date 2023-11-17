using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.Domain.Services;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain
{
    internal class DomainStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageFactory>(MessageFactory.Instance);
            services.AddSingleton<IQuickDict>(DictionaryService.Instance);
        }
    }
}