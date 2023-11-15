using System.Reflection;

using Mi.Domain.Shared;
using Mi.Domain.Shared.Attributes;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    /// <summary>
    /// 管道启动
    /// </summary>
    public class PipelineStartup
    {
        private static List<Type> _types;
        private static Lazy<PipelineStartup> Lazy => new(() => new PipelineStartup());
        public static PipelineStartup Instance => Lazy.Value;

        public PipelineStartup()
        {
            _types = GetAllTypes();
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var type in _types)
            {
                object? v = Activator.CreateInstance(type);
                type.GetMethod(nameof(Startup.ConfigureServices))?.Invoke(v, new object[] { services });
            }
        }

        /// <summary>
        /// 配置App
        /// </summary>
        /// <param name="app"></param>
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

        /// <summary>
        /// 反射获取继承 <see cref="Startup"/> 的启动类
        /// </summary>
        /// <returns></returns>
        private List<Type> GetAllTypes()
        {
            var list = new List<Type>();
            var baseType = typeof(Startup);
            foreach (var assembly in App.LoadAssemblies())
            {
                var types = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(baseType)).ToList();
                list.AddRange(types);
            }
            return list;
        }
    }
}