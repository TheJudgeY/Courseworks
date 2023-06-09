using Task = System.Threading.Tasks.Task;
using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserProjectRoleService : IGenericService<UserProjectRole>
    {
        Task CreateTableRow(Project project, User user);
        Task<List<UserProjectRole>> GetTableByProjectId(int id);
        Task SetUserRole(Project project, User user, Duty duty);
        Task<UserProjectRole> GetRowByProjectUser(Project project, User user);
    }
}
