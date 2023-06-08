using Core.Models;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Interfaces;
using Task = System.Threading.Tasks.Task;
using System.Runtime.CompilerServices;
using Helpers;
using Core.Enums;

namespace BLL.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IProjectService _projectService;
        private readonly IUserProjectRoleService _userProjectRoleService;
        public UserService(IStorage<User> storage, IProjectService projectService, IUserProjectRoleService userProjectRoleService) : base(storage)
        {
            _projectService = projectService;
            _userProjectRoleService = userProjectRoleService;
        }

        public async Task<Duty> GetUserDutyByIds(User user, Project project)
        {
            var table = await _userProjectRoleService.GetAll();

            foreach (UserProjectRole row in table)
            {
                if (row.Project.Id == project.Id && row.User.Id == user.Id)
                {
                    return row.Duty;
                }
            }
            return Duty.Unassigned;
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
