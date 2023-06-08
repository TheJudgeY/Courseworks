using Core.Models;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Interfaces;
using Task = System.Threading.Tasks.Task;
using System.Runtime.CompilerServices;
using Helpers;

namespace BLL.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IStorage<User> storage) : base(storage)
        {
        }

        public async Task<bool> Authenticate(User user, string password)
        {
            if (PasswordHasher.VerifyPassword(password, user.PasswordHashed))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
