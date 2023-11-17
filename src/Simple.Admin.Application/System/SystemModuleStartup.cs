using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.Application.Contracts.System.Models.Dict;
using Simple.Admin.Application.Contracts.System.Models.Function;
using Simple.Admin.Application.Contracts.System.Models.Log;
using Simple.Admin.Application.Contracts.System.Models.Role;
using Simple.Admin.Application.Contracts.System.Models.Tasks;
using Simple.Admin.Domain.Entities.System;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Application.System
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
                conf.CreateMap<SysTask, TaskItem>();
            });
        }
    }
}