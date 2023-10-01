using System.Reflection;

using Mi.Domain.Shared.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.Domain.PipelineConfiguration
{
    public static class InterfaceInjectionSetup
    {
        public static void AddAutomaticInjection(this IServiceCollection services)
        {
            foreach (var assembly in ServiceManager.LoadAssemblies())
            {
                JoinAssembly(services, assembly);
            }
        }

        private static readonly Type _scopedType = typeof(IScoped);
        private static readonly Type _singletonType = typeof(ISingleton);
        private static readonly Type _transientType = typeof(ITransient);
        private static readonly List<Type> _containTypes = new List<Type> { _scopedType, _singletonType, _transientType };

        private static void JoinAssembly(IServiceCollection services, Assembly assembly)
        {
            if (assembly == null) return;

            var types = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && _containTypes.Any(t => x.IsAssignableTo(t))).ToList();
            foreach (var type in types)
            {
#if DEBUG
                if (type.FullName!.Contains("Captcha"))
                {
                    Console.WriteLine("find Captcha.");
                }
#endif
                Add(services, type);
            }
        }

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