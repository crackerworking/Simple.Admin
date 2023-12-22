using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Simple.Admin.Domain.Services;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Attributes;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain
{
    [Sort(1)]
    internal class DomainStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageFactory>(MessageFactory.Instance);
            services.AddSingleton<IQuickDict>(DictionaryService.Instance);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(App.Configuration.GetSection("JWT")["ClockSkew"])),
                        ValidateIssuerSigningKey = true,
                        ValidAudience = App.Configuration.GetSection("JWT")["ValidAudience"],
                        ValidIssuer = App.Configuration.GetSection("JWT")["ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(App.Configuration.GetSection("JWT")["IssuerSigningKey"]!))
                    };
                });
        }
    }
}