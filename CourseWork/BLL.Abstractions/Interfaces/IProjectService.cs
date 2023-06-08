using Core.Models;
using Task = System.Threading.Tasks.Task;

namespace BLL.Abstractions.Interfaces
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<Project> CreateProject(Project project);
        Task<Project> GetProjectByTask(Core.Models.Task task);
    }
}
