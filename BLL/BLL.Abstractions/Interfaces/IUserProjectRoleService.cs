using BLL.Abstractions.Models;
using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserProjectRoleService : IGenericService<UserProjectRole>
    {
        Task CreateTableRow(Project project, UserServiceModel user);
        Task<List<UserProjectRole>> GetTableByProjectId(int id);
        Task SetUserRole(ProjectServiceModel project, UserServiceModel user, Duty duty);
    }
}
