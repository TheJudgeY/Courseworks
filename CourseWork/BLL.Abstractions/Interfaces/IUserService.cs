using Core.Enums;
using Core.Models;
using Task = System.Threading.Tasks.Task;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<bool> Authenticate(User user, string password);
        Task<Duty> GetUserDutyByIds(User user, Project project);
    }
}
