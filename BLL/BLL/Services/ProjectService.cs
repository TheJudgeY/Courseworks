using Core.Models;
using Core.Enums;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        public ProjectService(IStorage<Project> storage) : base(storage)
        { }

        public async Task CreateProject(string name, string descr, List<User> availibleUsers)
        {
            Project newProject = new Project()
            {
                Name = name,
                Description = descr,
                DevelopingStatus = Status.Planned,
                AssignedUsers = null
            };

            await Add(newProject);
        }
    }
}
