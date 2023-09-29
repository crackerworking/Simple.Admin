using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public class ServiceManager
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private static IServiceProvider Provider { get; set; }

        private static IServiceCollection Services { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public static TService Get<TService>() where TService : class
        {
            return Provider.GetRequiredService<TService>();
        }
    }
}