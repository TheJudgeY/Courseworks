using BLL.Abstractions.Models;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<ProjectServiceModel> CreateProject(ProjectServiceModel model);
        Task<List<ProjectServiceModel>> GetProjects();
        Task<ProjectServiceModel> GetProjectById(int id);
        Task UpdateProject(ProjectServiceModel projectModel);
    }
}
