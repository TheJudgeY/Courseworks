using Core.Enums;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task CreateUser(User user);
        Task<bool> Authenticate(User user, string password);
    }
}
