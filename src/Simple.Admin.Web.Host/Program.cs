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
using Simple.Admin.Domain.Tasks;
using Simple.Admin.Domain.User;
using Simple.Admin.Web.Host.Filter;
using Simple.Admin.Web.Host.Middleware;

const string CORS = "CustomCors";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(c =>
{
    c.EnableDetailedErrors = true;
    c.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    c.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
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

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(CORS);
app.UseRouting();
app.UseMiddleware<MiHeaderMiddleware>();

app.UseAuthentication();
app.UseMiddleware<UserMiddleware>();
app.UseAuthorization();

app.MapControllerRoute("api-router", "/api/{controller}/{action}");

app.MapHub<NoticeHub>("/noticeHub");

SystemTaskScheduler.Instance.Run();
StringHelper.Information();
app.Run();

static void ConfigureService(IServiceCollection services, IConfiguration configuration)
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

    // Cors
    services.AddCors(options =>
    {
        options.AddPolicy(CORS, conf => conf.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(configuration["AllowedCorsOrigins"]!.Split(',', StringSplitOptions.RemoveEmptyEntries)));
    });
}