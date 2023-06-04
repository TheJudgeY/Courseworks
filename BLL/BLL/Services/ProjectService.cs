using Core.Models;
using Core.Enums;
using DAL.Abstractions.Interfaces;
using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Models;
using System.Reflection;

namespace BLL.Services
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IUserProjectRoleService _userProjectRoleService;
        private readonly IUserService _userService;

        public ProjectService(IStorage<Project> storage, IAssignmentService assignmentService, IUserProjectRoleService userProjectRoleService, IUserService userService) : base(storage)
        {
            _assignmentService = assignmentService;
            _userProjectRoleService = userProjectRoleService;
            _userService = userService;
        }

        public async Task<ProjectServiceModel> CreateProject(ProjectServiceModel model)
        {
            Project newProject = new Project()
            {
                Name = model.Name,
                Description = model.Description
            };

            await Add(newProject);
            model.Id = newProject.Id;

            foreach (AssignmentServiceModel assignmentModel in model.Assignments)
            {
                Assignment assignment = await _assignmentService.ServiceModelConverter(assignmentModel);
                await _assignmentService.Add(assignment);
            }

            foreach (UserServiceModel userModel in model.Users)
            {
                await _userProjectRoleService.CreateTableRow(newProject, userModel);
            }

            return model;
        }

        public async Task<List<ProjectServiceModel>> GetProjects()
        {
            var projects = await GetAll();
            List<ProjectServiceModel> result = new List<ProjectServiceModel>();

            foreach (var project in projects)
            {
                List<UserServiceModel> users = await _userService.GetUsersByProjectId(project.Id);
                List<AssignmentServiceModel> assignments = await _assignmentService.GetAssignmentsByProjectId(project.Id);

                ProjectServiceModel model = new ProjectServiceModel()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Assignments = assignments,
                    Users = users
                };

                result.Add(model);
            }

            return result;
        }

        public async Task<ProjectServiceModel> GetProjectById(int id)
        {
            var project = await GetById(id);
            var result = await GetProjects();

            foreach (ProjectServiceModel model in result.Where(p => p.Id == project.Id))
            {
                return model;
            }

            throw new Exception("ProjectServiceModel doesn't match Project");
        }

        public async Task UpdateProject(ProjectServiceModel projectModel)
        {
            var projects = await GetAll();
            var table = await _userProjectRoleService.GetAll();
            List<UserProjectRole> relationTableFiltered = new List<UserProjectRole>();

            foreach (Project project in projects.Where(p => p.Id == projectModel.Id))
            {
                project.Name = projectModel.Name;
                project.Description = projectModel.Description;

                await Update(project.Id, project);

                foreach (UserProjectRole row in table.Where(r => r.ProjectId == project.Id))
                {
                    relationTableFiltered.Add(row);
                }

                foreach (UserServiceModel user in projectModel.Users)
                {
                    bool userExistsInRelationTable = relationTableFiltered.Any(row => row.UserId == user.Id);
                    if (!userExistsInRelationTable)
                    {
                        await _userProjectRoleService.CreateTableRow(project, user);
                    }
                }
            }
        }
    }
}
