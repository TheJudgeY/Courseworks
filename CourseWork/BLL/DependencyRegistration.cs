using BLL.Abstractions.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using DAL;

namespace BLL
{
    public class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IProjectService, ProjectService>();

            DAL.DependencyRegistration.RegisterRepositories(services);
        }
    }
}
