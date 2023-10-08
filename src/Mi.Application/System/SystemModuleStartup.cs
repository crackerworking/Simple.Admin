using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Application.System
{
    internal class SystemModuleStartup : Startup
    {
        public override void Configure(IApplicationBuilder app)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(conf =>
            {
                conf.CreateMap<SysFunctionFull, SysFunction>();
                conf.CreateMap<SysFunction, SysFunctionFull>();
                conf.CreateMap<SysFunction, FunctionOperation>();
                conf.CreateMap<SysLoginLog, SysLoginLogFull>();
                conf.CreateMap<SysLog, SysLogFull>();
                conf.CreateMap<SysRole, SysRoleFull>();
            });
        }
    }
}