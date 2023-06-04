using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Models;
using Core.Enums;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class TesterUI
    {
        private readonly AssignmentUI _assignmentUI;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public TesterUI(AssignmentUI assignmentUI, IProjectService projectService, IUserService userService) 
        {
            _assignmentUI = assignmentUI;
            _projectService = projectService;
            _userService = userService;
        }
        public async Task PerformOperationsAsync(int projectId, int userId)
        {
            ProjectServiceModel project = await _projectService.GetProjectById(projectId);
            UserServiceModel user = await _userService.GetUserById(userId);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Please choose one of the following options:\n" +
                "1. Choose Assignment\n" +
                "2. Exit");

                string? input = Console.ReadLine();
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
                        Console.WriteLine("Invalid input");
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
                "1. Hand Assignment\n" +
                "2. Upload File\n" +
                "3. Exit");

            bool exit = false;
            string input = Console.ReadLine();
            while (!exit)
            {
                switch (input)
                {
                    case "1":
                        await _assignmentUI.ChangeExecutor(assignment, project);
                        break;
                    case "2":
                        await _assignmentUI.AddFile(assignment);
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
