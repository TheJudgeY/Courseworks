using Core.Models;
using BLL.Abstractions.Interfaces;
using DAL.Abstractions.Interfaces;
using Core.Enums;
using BLL.Abstractions.Models;
using System.Numerics;
using System.Reflection;

namespace BLL.Services
{
    public class AssignmentService : GenericService<Assignment>, IAssignmentService
    {
        private readonly IFileAssignmentService _fileAssignmentService;
        public AssignmentService(IStorage<Assignment> storage, IFileAssignmentService fileAssignmentService) : base(storage)
        {
            _fileAssignmentService = fileAssignmentService;
        }

        public async Task CreateAssignment(AssignmentServiceModel assignment, int projectId, int userId)
        {
            Assignment newAssignment = new Assignment()
            {
                Name = assignment.Name,
                Description = assignment.Description,
                EstimatedTime = assignment.EstimatedTime,
                Priority = assignment.Priority,
                Status = assignment.Status,
                ProjectId = projectId,
                UserId = userId
            };

            assignment.Id = assignment.Id;
            await Add(newAssignment);
        }

        public async Task<Assignment> ServiceModelConverter(AssignmentServiceModel model)
        {
            var assignments = await GetAll();

            foreach (Assignment assignment in assignments.Where(a => a.Id == model.Id))
            {
                await _fileAssignmentService.CreateRow(model);

                assignment.Name = model.Name;
                assignment.Description = model.Description;
                assignment.EstimatedTime = model.EstimatedTime;
                assignment.Priority = model.Priority;
                assignment.Status = model.Status;

                return assignment;
            }

            throw new Exception("AssignmentServiceModel doesn't match Assignment");
        }

        public async Task UpdateAssignment(AssignmentServiceModel model)
        {
            Assignment assignment = await ServiceModelConverter(model);
            await Update(assignment.Id, assignment);
        }

        public async Task UpdateUserAssignment(AssignmentServiceModel model, UserServiceModel user)
        {
            Assignment assignment = await ServiceModelConverter(model);
            foreach (AssignmentServiceModel assignmentModel in user.Assignments.Where(a => a.Id == model.Id))
            {
                assignment.UserId = user.Id;
            }
            await Update(assignment.Id, assignment);
        }

        public async Task<List<AssignmentServiceModel>> GetAssignmentsByProjectId(int id)
        {
            var assignments = await GetAll();

            List<AssignmentServiceModel> result = new List<AssignmentServiceModel>();
            foreach (Assignment assignment in assignments.Where(a => a.ProjectId == id))
            {
                List<FileCommentModelService> files = await _fileAssignmentService.GetFileCommentByAssignmentId(assignment.Id);
                AssignmentServiceModel model = new AssignmentServiceModel() 
                { 
                    Id = assignment.Id,
                    Name = assignment.Name,
                    Description = assignment.Description,
                    EstimatedTime = assignment.EstimatedTime,
                    Priority = assignment.Priority,
                    Status = assignment.Status,
                    Files = files
                };

                result.Add(model);
            }

            return result;
        }

        public async Task<List<AssignmentServiceModel>> GetAssignmentsByUserId(int id)
        {
            var assignments = await GetAll();

            List<AssignmentServiceModel> result = new List<AssignmentServiceModel>();
            foreach (Assignment assignment in assignments.Where(a => a.UserId == id))
            {
                List<FileCommentModelService> files = await _fileAssignmentService.GetFileCommentByAssignmentId(assignment.Id);
                AssignmentServiceModel model = new AssignmentServiceModel()
                {
                    Id = assignment.Id,
                    Name = assignment.Name,
                    Description = assignment.Description,
                    EstimatedTime = assignment.EstimatedTime,
                    Priority = assignment.Priority,
                    Status = assignment.Status,
                    Files = files
                };

                result.Add(model);
            }

            return result;
        }

        public async Task<AssignmentServiceModel> GetAssignmentById(int id, int projectId)
        {
            var result = await GetById(id);

            var assignments = await GetAssignmentsByProjectId(projectId);
            foreach (AssignmentServiceModel assignment in assignments.Where(a => a.Id == result.Id))
            {
                return assignment;
            }

            throw new Exception("AssignmentServiceModel doesn't match Assignment");
        }

        public async Task<bool> CheckAvailibilityForUser(UserServiceModel user, AssignmentServiceModel assignmentModel)
        {
            var assignment = await ServiceModelConverter(assignmentModel);

            if(assignment.UserId == user.Id) 
            { 
                return true;
            }

            return false;
        }
    }
}