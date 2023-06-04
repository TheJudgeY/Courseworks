using BLL.Abstractions.Interfaces;
using Core.Models;
using BLL.Abstractions.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class StateManagerUI
    {
        private readonly ProjectUI _projectUI;
        private readonly AssignmentUI _assignmentUI;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        public StateManagerUI(ProjectUI projectUI, AssignmentUI assignmentUI, IProjectService projectService, IUserService userService)
        {
            _projectUI = projectUI;
            _assignmentUI = assignmentUI;
            _projectService = projectService;
            _userService = userService;
        }

        public async Task PerformOperationsAsync(int userId, int projectId)
        {
            var user = await _userService.GetUserById(userId);
            var project = await _projectService.GetProjectById(projectId);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Greetings!\n" +
                "===========================================\n" +
                "Please choose one of the following options:\n" +
                "1. Add Project\n" +
                "2. Edit current Project\n" +
                "3. Exit");

                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await _projectUI.CreateNewProject(user);
                        break;
                    case "2":
                        await ProjectInteraction(project, user);
                        break;
                    case "3":
                        Console.Clear();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid operation number.");
                        break;
                }
            }
        }

        private async Task ProjectInteraction(ProjectServiceModel project, UserServiceModel user)
        {
            Console.Clear();
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Edit Project Name\n" +
                "2. Edit Project Description\n" +
                "3. Create Assignment\n" +
                "4. Edit Assignments\n" +
                "5. Edit Workers\n" +
                "6. Exit");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Please enter new name:");
                    project.Name = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine("Please enter new Description:");
                    project.Description = Console.ReadLine();
                    break;
                case "3":
                    AssignmentServiceModel assignment = await _assignmentUI.CreateAssignment(user, project);
                    await _assignmentUI.ChangeExecutor(assignment, project);
                    break;
                case "4":
                    await AssignmentInteraction(project, user);
                    break;
                case "5":
                    await _projectUI.UpdateProjectWorkers(project);
                    break;
                case "6":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Invalid operation number.");
                    break;
            }

            await _projectService.UpdateProject(project);
        }

        private async Task AssignmentInteraction(ProjectServiceModel project, UserServiceModel user)
        {
            AssignmentServiceModel assignment = await _assignmentUI.ChooseAssignment(project, user);
            if (assignment != null)
            {
                await _assignmentUI.UpdateAssignmentStateManager(assignment, project);
            }

            Console.WriteLine("No acess to the assignment. No actions were performed");
        }
    }
}