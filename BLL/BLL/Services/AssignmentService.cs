using Core.Models;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class AssignmentService : GenericService<Assignment>, IAssignmentService
    {
        public AssignmentService(IStorage<Assignment> storage) : base(storage)
        {
        }

        public async Task CreateAssignment(string name, string desc, DateTime deadline, List<User> workers, User currentUser)
         {
            Assignment assignment = new Assignment()
            {
                Name = name,
                Description = desc,
                Deadline = deadline,
                Workers = workers,
                ResponsibleUser = currentUser
            };

            await Add(assignment);
         }
    }
}