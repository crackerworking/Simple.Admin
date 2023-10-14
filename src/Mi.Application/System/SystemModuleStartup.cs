using Mi.Application.Contracts.System.Models.Dict;
using Mi.Application.Contracts.System.Models.Function;
using Mi.Application.Contracts.System.Models.Log;
using Mi.Application.Contracts.System.Models.Role;
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
                conf.CreateMap<FunctionOperation, SysFunction>();
                conf.CreateMap<SysLoginLog, SysLoginLogFull>();
                conf.CreateMap<SysLog, SysLogFull>();
                conf.CreateMap<SysRole, SysRoleFull>();
                conf.CreateMap<SysDict, SysDictFull>();
                conf.CreateMap<DictPlus, SysDict>();
            });
        }
    }
}