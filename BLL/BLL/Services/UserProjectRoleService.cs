using Core.Models;
using Core.Enums;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Models;

namespace BLL.Services
{
    public class UserProjectRoleService : GenericService<UserProjectRole>, IUserProjectRoleService
    {
        public UserProjectRoleService(IStorage<UserProjectRole> storage) : base(storage)
        {
        }

        public async Task CreateTableRow(Project project, UserServiceModel user)
        {
            UserProjectRole newTableRow = new UserProjectRole()
            {
                ProjectId = project.Id,
                UserId = user.Id,
                Duty = Duty.Unassigned
            };

            await Add(newTableRow);
        }

        public async Task SetUserRole(ProjectServiceModel project, UserServiceModel user, Duty duty)
        {
            var table = await GetAll();

            foreach (var row in table.Where(r => r.ProjectId == project.Id && r.UserId == user.Id)) 
            { 
                row.Duty = duty;
                await Update(row.Id, row);
            }
        }

        public async Task<List<UserProjectRole>> GetTableByProjectId(int id)
        {
            var table = await GetAll();
            List<UserProjectRole> newTable = new List<UserProjectRole>();

            foreach (var row in table.Where(t => t.ProjectId == id)) 
            { 
                newTable.Add(row);
            }

            return newTable;
        }
    }
}
