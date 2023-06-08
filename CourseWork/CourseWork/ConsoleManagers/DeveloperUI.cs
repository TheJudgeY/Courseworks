using Core.Models;
using Task = System.Threading.Tasks.Task;

namespace UI.ConsoleManagers
{
    public class DeveloperUI
    {
        private readonly TaskUI _taskUI;

        public DeveloperUI(TaskUI taskUI)
        {
            _taskUI = taskUI;
        }
        public async Task PerformOperationsAsync(User user, Project project)
        {
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
                await UpdateAssignment(task, project);
            }

            Console.WriteLine("No acess to the assignment. No actions were performed");
        }

        private async Task UpdateAssignment(Core.Models.Task task, Project project)
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Hand Assignment\n" +
                "2. Upload File\n" +
                "3. Open File Folder\n" +
                "4. Exit");

            bool exit = false;
            string input = Console.ReadLine();
            while (!exit)
            {
                switch (input)
                {
                    case "1":
                        User chosenUser = await _taskUI.GetUserByProjects(project);
                        await _taskUI.ChangeExecutor(task, chosenUser);
                        exit = true;
                        break;
                    case "2":
                        await _taskUI.AddFile(task);
                        exit = true;
                        break;
                    case "3":
                        await _taskUI.OpenFileAttachments(task);
                        exit = true;
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
