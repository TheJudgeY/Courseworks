using BLL.Abstractions.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileAssignmentService, FileAssignmentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserProjectRoleService, UserProjectRoleService>();
            services.AddScoped<IAssignmentService, AssignmentService>();

            DAL.DependencyRegistration.RegisterRepositories(services);
        }
    }
}
