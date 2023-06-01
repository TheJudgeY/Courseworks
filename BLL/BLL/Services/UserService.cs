using Core.Enums;
using Core.Models;
using Helpers;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IStorage<User> storage) : base(storage) 
        { 
        }

        public async Task CreateUser(User newUser)
        {
            await Add(newUser);
        }  
        public async Task<bool> Authenticate(User user, string password)
        {
            if (PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
