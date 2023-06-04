using BLL.Abstractions.Models;
using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<UserServiceModel> CreateUser(UserServiceModel model);
        Task<bool> Authenticate(UserServiceModel user, string password);
        Task<List<UserServiceModel>> GetUsersByProjectId(int id);
        Task<List<UserServiceModel>> GetUsers();
        Task<Duty> GetUserDutyByIds(int userId, int projectId);
        Task<UserServiceModel> GetUserById(int id);
    }
}
