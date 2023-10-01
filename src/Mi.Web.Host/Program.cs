using Mi.DataDriver;
using Mi.DataDriver.EntityFrameworkCore;
using Mi.Domain.PipelineConfiguration;
using Mi.Domain.Shared.Models;
using Mi.Domain.User;

namespace Mi.Web.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            PipelineStartup.Instance.ConfigureService(builder.Services);
            ConfigureService(builder.Services, builder.Configuration);

            var app = builder.Build();
            ServiceManager.SetProvider(app.Services);
            PipelineStartup.Instance.Configure(app);

            app.UseStaticFiles();

            app.MapRazorPages();
            app.MapControllerRoute("api-router", "/api/[Controller]/[Action]");
            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            // DB & Repository
            services.AddMiDbContext(configuration.GetConnectionString("Sqlite")!);
            services.AddRepository();

            // CurrentUser
            services.AddCurrentUser();

            // cache
            services.AddMemoryCache();

            // AddAutomaticInjection
            services.AddAutomaticInjection();

            services.AddScoped<MiHeader>();
        }
    }
}