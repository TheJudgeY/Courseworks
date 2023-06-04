using Core.Enums;
using Core.Models;
using BLL.Abstractions.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IAssignmentService : IGenericService<Assignment>
    {
        Task CreateAssignment(AssignmentServiceModel assignment, int projectId, int userId);
        Task<Assignment> ServiceModelConverter(AssignmentServiceModel model);
        Task<List<AssignmentServiceModel>> GetAssignmentsByProjectId(int id);
        Task<List<AssignmentServiceModel>> GetAssignmentsByUserId(int id);
        Task<AssignmentServiceModel> GetAssignmentById(int id, int projectId);
        Task UpdateAssignment(AssignmentServiceModel model);
        Task UpdateUserAssignment(AssignmentServiceModel model, UserServiceModel user);
        Task<bool> CheckAvailibilityForUser(UserServiceModel user, AssignmentServiceModel assignmentModel);
    }
}
