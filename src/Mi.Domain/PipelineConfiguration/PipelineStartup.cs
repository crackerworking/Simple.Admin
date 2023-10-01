using System.Reflection;

using Mi.Domain.Shared.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public class PipelineStartup
    {
        private static List<Type> _types;
        private static Lazy<PipelineStartup> Lazy => new(() => new PipelineStartup());
        public static PipelineStartup Instance => Lazy.Value;

        public PipelineStartup()
        {
            _types = GetAllTypes();
        }

        public void ConfigureService(IServiceCollection services)
        {
            foreach (var type in _types)
            {
                object? v = Activator.CreateInstance(type);
                type.GetMethod(nameof(Startup.ConfigureService))?.Invoke(v, new object[] { services });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            var dict = new Dictionary<Type, int>();
            foreach (var type in _types)
            {
                var sort = type.GetCustomAttribute<SortAttribute>();
                if (sort == null)
                {
                    dict.TryAdd(type, 0);
                }
                else
                {
                    dict.TryAdd(type, sort.Value);
                }
            }
            foreach (var item in dict.OrderBy(i => i.Value))
            {
                object? v = Activator.CreateInstance(item.Key);
                if (v == null) continue;
                item.Key.GetMethod(nameof(Startup.Configure))?.Invoke(v, new object[] { app });
            }
        }

        private List<Type> GetAllTypes()
        {
            var list = new List<Type>();
            var baseType = typeof(Startup);
            foreach (var assembly in ServiceManager.LoadAssemblies())
            {
                var types = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(baseType)).ToList();
                list.AddRange(types);
            }
            return list;
        }
    }
}