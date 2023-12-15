using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using NSwag;

using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.ControllerLibrary
{
    [Sort(99)]
    internal class ControllerStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
            if (App.IsDevelopment)
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(options =>
            {
                options.PostProcess = document =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Mi API",
                        Description = "An ASP.NET Core Web API for admin",
                        TermsOfService = "https://github.com/cracker-beep",
                        Contact = new OpenApiContact
                        {
                            Name = "Contact us",
                            Url = "crackerwork@outlook.com"
                        }
                    };
                };
                options.UseControllerSummaryAsTagDescription = true;
            });
        }
    }
}