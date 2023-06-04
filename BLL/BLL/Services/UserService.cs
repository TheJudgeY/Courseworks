using Core.Enums;
using Core.Models;
using Helpers;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Models;

namespace BLL.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserProjectRoleService _userProjectRoleService;
        private readonly IAssignmentService _assignmentService;
        private readonly INotificationService _notificationService;
        public UserService(IStorage<User> storage, IUserProjectRoleService userProjectRoleService, IAssignmentService assignmentService, INotificationService notificationService) : base(storage)
        {
            _userProjectRoleService = userProjectRoleService;
            _assignmentService = assignmentService;
            _notificationService = notificationService;
        }

        public async Task<UserServiceModel> CreateUser(UserServiceModel userModel)
         {
            User newUser = new User()
            {
                Username = userModel.Username,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                PasswordHash = userModel.PasswordHashed,
                Email = userModel.Email,
            };

            await Add(newUser);

            userModel.Id = newUser.Id;
            return userModel;
        }

        public async Task<List<UserServiceModel>> GetUsersByProjectId(int id)
        {
            List<UserServiceModel> models = new List<UserServiceModel>();
            var result = await _userProjectRoleService.GetAll();
            var assignments = await _assignmentService.GetAssignmentsByProjectId(id);
            List<AssignmentServiceModel> assignmentServiceModels = new List<AssignmentServiceModel>();

            foreach (var row in result.Where(i => i.ProjectId == id))
            {
                foreach (AssignmentServiceModel assignment in assignments)
                {
                    var assignmentDB = await _assignmentService.ServiceModelConverter(assignment);
                    if (assignmentDB.Id == row.UserId)
                    {
                        assignmentServiceModels.Add(assignment);
                    }
                }
                User user = await GetById(row.UserId);
                List<string> notifications = await _notificationService.GetNotificationsByUserId(row.UserId);

                UserServiceModel model = new UserServiceModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PasswordHashed = user.PasswordHash,
                    Email = user.Email,
                    Assignments = assignmentServiceModels,
                    Notifications = notifications
                };

                models.Add(model);
            }

            return models;
        }

        public async Task<List<UserServiceModel>> GetUsers()
        {
            var users = await GetAll();
            var models = new List<UserServiceModel>();

            foreach (User user in users)
            {
                List<AssignmentServiceModel> assignments = await _assignmentService.GetAssignmentsByUserId(user.Id);
                List<string> notifications = await _notificationService.GetNotificationsByUserId(user.Id);
                UserServiceModel model = new UserServiceModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PasswordHashed = user.PasswordHash,
                    Email = user.Email,
                    Assignments = assignments,
                    Notifications = notifications
                };

                models.Add(model);
            }

            return models;
        }

        public async Task<Duty> GetUserDutyByIds(int userId, int projectId)
        {
            var table = await _userProjectRoleService.GetAll();

            foreach (UserProjectRole row in table)
            {
                if (row.ProjectId == projectId && row.UserId == userId)
                {
                    return row.Duty;
                }
            }
            return Duty.Unassigned;
        }

        public async Task<UserServiceModel> GetUserById(int id)
        {
            var user = await GetById(id);

            List<AssignmentServiceModel> assignments = await _assignmentService.GetAssignmentsByUserId(id);
            List<string> notifications = await _notificationService.GetNotificationsByUserId(id);

            UserServiceModel model = new UserServiceModel()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHashed = user.PasswordHash,
                Email = user.Email,
                Assignments = assignments,
                Notifications = notifications
            };

            return model;
        }

        public async Task UpdateUser(UserServiceModel model)
        {
            User user = await UserModelConverter(model);
            await Update(user.Id, user);
        }

        public async Task<User> UserModelConverter(UserServiceModel model)
        {
            var users = await GetAll();

            foreach (User user in users.Where(u => u.Id == model.Id)) 
            {
                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PasswordHash = model.PasswordHashed;
                user.Email = model.Email;

                return user;
            }

            throw new Exception("UserServiceModel doesn't match User");
        }

        public async Task<bool> Authenticate(UserServiceModel user, string password)
        {
            if (PasswordHasher.VerifyPassword(password, user.PasswordHashed))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
