using BLL.Abstractions.Interfaces;
using BLL.Services;
using Core.Models;
using Helpers.Validators;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace UI.ConsoleManagers
{
    public class TesterUI
    {
        private readonly TaskUI _taskUI;
        private readonly ProjectUI _projectUI;
        private readonly IUserService _userService;

        public TesterUI(TaskUI taskUI, ProjectUI projectUI, IUserService userService)
        {
            _taskUI = taskUI;
            _projectUI = projectUI;
            _userService = userService;
        }
        public async Task PerformOperationsAsync(int userId, int projectId)
        {
            var user = await _userService.GetById(userId);
            var project = await _projectUI.GetByIdAsync(projectId);

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

        private async Task AssignmentInteraction(Project project, User user)
        {
            Core.Models.Task task = await _taskUI.ChooseTask(project, user);
            if (task != null)
            {
                await UpdateAssignment(task, project, user);
            }

            Console.WriteLine("No acess to the assignment. No actions were performed");
        }

        private async Task UpdateAssignment(Core.Models.Task task, Project project, User user)
        {
            Console.Clear();
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Hand Assignment\n" +
                "2. Upload File\n" +
                "3. Open File Folder\n" +
                "4. Exit");

            bool exit = false;
            string input = StringValidator.ReadLineOrDefault();
            while (!exit)
            {
                switch (input)
                {
                    case "1":
                        User chosenUser = await _taskUI.GetUserByProjects(project);
                        await _taskUI.ChangeExecutor(task, chosenUser, user);
                        break;
                    case "2":
                        await _taskUI.AddFile(task);
                        break;
                    case "3":
                        await _taskUI.OpenFileAttachments(task);
                        break;
                    case "4":
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
