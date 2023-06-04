using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ConsoleManagers;
using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = DependencyRegistration.Register();

            using (var scope = serviceProvider.CreateScope())
            {
                var userUI = scope.ServiceProvider.GetService<UserUI>();
                userUI.PerformOperationsAsync().Wait();
            }
        }
    }
}
