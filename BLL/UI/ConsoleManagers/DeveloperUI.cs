using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Models;
using Core.Enums;
using Core.Models;
using Helpers.Validators;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class DeveloperUI
    {
        private readonly AssignmentUI _assignmentUI;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public DeveloperUI (AssignmentUI assignmentUI, IProjectService projectService, IUserService userService)
        {
            _assignmentUI = assignmentUI;
            _projectService = projectService;
            _userService = userService;
        }

        public async Task PerformOperationsAsync(int projectId, int userId)
        {
            var project = await _projectService.GetProjectById(projectId);
            var user = await _userService.GetUserById(userId);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Please choose one of the following options:\n" +
                "1. Assignments\n" +
                "2. Exit");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await AssignmentInteraction(project, user);
                        break;
                    case "2":
                        Console.Clear();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid operation number.");
                        break;
                }
            }
        }

        private async Task AssignmentInteraction(ProjectServiceModel project, UserServiceModel user)
        {
            AssignmentServiceModel assignment = await _assignmentUI.ChooseAssignment(project, user);
            if (assignment != null)
            {
                await UpdateAssignment(assignment, project);
            }

            Console.WriteLine("No acess to the assignment. No actions were performed");
        }

        private async Task UpdateAssignment(AssignmentServiceModel assignment, ProjectServiceModel project)
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Upload File\n" +
                "2. Hand Assignment to a different worker\n" +
                "3. Exit");

            bool exit = false;
            string input = Console.ReadLine();
            while (!exit) 
            {
                switch (input)
                {
                    case "1":
                        await _assignmentUI.AddFile(assignment);
                        exit = true;
                        break;
                    case "2":
                        await _assignmentUI.ChangeExecutor(assignment, project);
                        exit = true;
                        break;
                    case "3":
                        Console.Clear();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }
    }
}
