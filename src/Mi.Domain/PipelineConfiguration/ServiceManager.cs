using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public class ServiceManager
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private static IServiceProvider Provider;

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public static TService Get<TService>() where TService : class
        {
            return Provider.GetRequiredService<TService>();
        }

        public static void SetProvider(IServiceProvider serviceProvider)
        {
            if (Provider == null) Provider = serviceProvider;
        }

        public static List<Assembly> LoadAssemblies()
        {
            var list = new List<Assembly>
            {
                Assembly.Load("Mi.Application"),
                Assembly.Load("Mi.Application.Contracts"),
                Assembly.Load("Mi.DataDriver"),
                Assembly.Load("Mi.Domain"),
                Assembly.Load("Mi.Web.Host")
            };

            return list;
        }
    }
}