using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain.PipelineConfiguration
{
    /// <summary>
    /// 接口-实现形式注入
    /// </summary>
    public static class InterfaceInjectionSetup
    {
        /// <summary>
        /// 动态注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutomaticInjection(this IServiceCollection services)
        {
            foreach (var assembly in App.LoadAssemblies())
            {
                JoinAssembly(services, assembly);
            }
        }

        private static readonly Type _scopedType = typeof(IScoped);
        private static readonly Type _singletonType = typeof(ISingleton);
        private static readonly Type _transientType = typeof(ITransient);
        private static readonly List<Type> _containTypes = new List<Type> { _scopedType, _singletonType, _transientType };

        /// <summary>
        /// 加入程序集
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        private static void JoinAssembly(IServiceCollection services, Assembly assembly)
        {
            if (assembly == null) return;

            var types = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && _containTypes.Any(t => x.IsAssignableTo(t))).ToList();
            foreach (var type in types)
            {
                Add(services, type);
            }
        }

        /// <summary>
        /// 单个注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        private static void Add(IServiceCollection services, Type type)
        {
            var interfaceTypes = type.GetInterfaces().Where(t => !_containTypes.Contains(t)).ToList();
            if (interfaceTypes != null && interfaceTypes.Count > 0)
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    Execute(services, type, interfaceType);
                }
            }
            else
            {
                Execute(services, type, type);
            }
        }

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="implType"></param>
        /// <param name="serviceType"></param>
        private static void Execute(IServiceCollection services, Type implType, Type serviceType)
        {
            if (implType.IsAssignableTo(_scopedType))
                services.AddScoped(serviceType, implType);
            else if (implType.IsAssignableTo(_singletonType))
                services.AddSingleton(serviceType, implType);
            else if (implType.IsAssignableTo(_transientType))
                services.AddTransient(serviceType, implType);
        }
    }
}