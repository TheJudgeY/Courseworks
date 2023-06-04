using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ConsoleManagers;
using UI.Interfaces;

namespace UI
{
    public class DependencyRegistration
    {
        public static IServiceProvider Register()
        {
            var services = new ServiceCollection();

            services.AddScoped<UserUI>();
            services.AddScoped<AssignmentUI>();
            services.AddScoped<DeveloperUI>();
            services.AddScoped<ProjectUI>();
            services.AddScoped<StateManagerUI>();
            services.AddScoped<TesterUI>();
            services.AddScoped<DutyPermission>();

            foreach (Type type in typeof(IConsoleManager<>).Assembly.GetTypes()
                         .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                             .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsoleManager<>))))
            {
                Type interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsoleManager<>));
                services.AddScoped(interfaceType, type);
            }

            BLL.DependencyRegistration.RegisterServices(services);

            return services.BuildServiceProvider();
        }
    }
}
