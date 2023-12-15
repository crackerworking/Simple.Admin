using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Quartz.Impl;

using Simple.Admin.DataDriver;
using Simple.Admin.DataDriver.EntityFrameworkCore;
using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Helper;
using Simple.Admin.Domain.Hubs;
using Simple.Admin.Domain.Json;
using Simple.Admin.Domain.PipelineConfiguration;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Models.UI;
using Simple.Admin.Domain.Tasks;
using Simple.Admin.Domain.User;
using Simple.Admin.Web.Host.Filter;
using Simple.Admin.Web.Host.Middleware;

namespace Simple.Admin.Web.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
                opt.Filters.Add<GlobalActionFilterAttribute>();
            }).AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
                opt.JsonSerializerOptions.Converters.Add(new DateTimeFormatConverter());
                opt.JsonSerializerOptions.Converters.Add(new DateTimeNullableFormatConverter());
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, FuncAuthorizationMiddleware>();

            PipelineStartup.Instance.ConfigureServices(builder.Services);
            ConfigureService(builder.Services, builder.Configuration);

            var app = builder.Build();

            App.Running(app.Environment.IsDevelopment(), app.Environment.WebRootPath, app.Configuration, app.Services);

            PipelineStartup.Instance.Configure(app);

            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.UseStaticFiles();

            app.UseRouting();
            app.UseMiddleware<MiHeaderMiddleware>();

            app.UseAuthentication();
            app.UseMiddleware<UserMiddleware>();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute("api-router", "/api/{controller}/{action}");

            app.MapHub<NoticeHub>("/noticeHub");

            SystemTaskScheduler.Instance.Run();
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

            services.AddScoped(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                return httpContextAccessor.HttpContext!.Features.Get<MiHeader>() ?? new MiHeader();
            });

            // UI Config
            var uiConfig = configuration.GetSection("AdminUI");
            services.Configure<PaConfigModel>(uiConfig);

            // quartz
            services.AddSingleton(sp =>
            {
                var f = new StdSchedulerFactory();
                return f.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
            });

            // model validate
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = context.ModelState.FirstOrDefaultMsg();

                    return new JsonResult(Back.ParameterError(error));
                };
            });
        }
    }
}