using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public class ServiceManager
    {
        public static IServiceProvider Provider { get; private set; }

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
                Assembly.Load("Mi.ControllerLibrary"),
                Assembly.Load("Mi.Web.Host")
            };

            return list;
        }
    }
}