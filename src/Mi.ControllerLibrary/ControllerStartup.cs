using Mi.Domain.Shared;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.ControllerLibrary
{
    [Sort(99)]
    internal class ControllerStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
            if (App.IsDevelopment)
            {
                // Add OpenAPI 3.0 document serving middleware
                // Available at: http://localhost:<port>/swagger/v1/swagger.json
                app.UseOpenApi();

                // Add web UIs to interact with the document
                // Available at: http://localhost:<port>/swagger
                app.UseSwaggerUi3();
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument();
        }
    }
}