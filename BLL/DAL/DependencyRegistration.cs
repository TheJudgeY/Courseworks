using DAL.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DAL.Services;

namespace DAL
{
    public class DependencyRegistration
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IStorage<>), typeof(Storage<>));
        }
    }
}
