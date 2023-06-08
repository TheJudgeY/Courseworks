using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace BLL.Services
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        public ProjectService(IStorage<Project> storage) : base(storage)
        {
        }

        public async Task<Project> CreateProject(Project project)
        {
            await Add(project);

            return project;
        }

        public async Task<Project> GetProjectByTask(Core.Models.Task task)
        {
            var projects = await GetAll();

            foreach (var project in projects.Where(p => p.Tasks.Any(t => t.Id == task.Id)))
            {
                return project;
            }

            return null;
        }
    }
}
