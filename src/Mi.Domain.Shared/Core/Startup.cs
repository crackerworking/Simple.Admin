using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.Shared.Core
{
    /// <summary>
    /// 模拟.NET启动抽象类
    /// </summary>
    public abstract class Startup
    {
        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app);
    }
}