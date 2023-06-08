using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using Helpers.Validators;
using Task = System.Threading.Tasks.Task;

namespace UI.ConsoleManagers
{
    public class StateManagerUI
    {
        private readonly ProjectUI _projectUI;
        private readonly TaskUI _taskUI;
        private readonly IUserService _userService;    
        public StateManagerUI(ProjectUI projectUI, TaskUI taskUI, IUserService userService)
        {
            _projectUI = projectUI;
            _taskUI = taskUI;
            _userService = userService;
        }

        public async Task PerformOperationsAsync(User user, Project project)
        {
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

        private async Task ProjectInteraction(Project project, User user)
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
                    project.Name = StringValidator.ReadLineOrDefault();
                    break;
                case "2":
                    Console.WriteLine("Please enter new Description:");
                    project.Description = StringValidator.ReadLineOrDefault();
                    break;
                case "3":
                    await _taskUI.CreateTask(project, user);
                    break;
                case "4":
                    await TaskInteraction(project, user);
                    break;
                case "5":
                    await UpdateProjectWorkers(project);
                    break;
                case "6":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Invalid operation number.");
                    break;
            }

            await _projectUI.UpdateAsync(project.Id, project);
        }

        private async Task TaskInteraction(Project project, User user)
        {
            Core.Models.Task task = await _taskUI.ChooseTask(project, user);
            if (task != null)
            {
                await UpdateTaskStateManager(task, project, user);
            }

            Console.WriteLine("No acess to the assignment. No actions were performed");
        }

        public async Task UpdateProjectWorkers(Project project)
        {
            Console.Clear();
            Console.WriteLine("Please enter one of the following options:\n" +
                "1. Remove Worker from Project\n" +
                "2. Assign Worker for Project\n" +
                "3. Assign duty\n" +
                "4. Exit");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    User userRemove = await _taskUI.GetUserByProjects(project);
                    if (userRemove != null)
                    {
                        project.Workers.Remove(userRemove);
                    }
                    Console.WriteLine("Something went wrong. No user removed");
                    break;
                case "2":
                    User userAdd = await _taskUI.GetUserAsync();
                    if (userAdd != null)
                    {
                        project.Workers.Add(userAdd);
                    }
                    break;
                case "3":
                    User worker = await _taskUI.GetUserByProjects(project);
                    await ChangeDuty(worker);
                    break;
                case "4":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Please enter a valid number");
                    break;
            }

            await _projectUI.UpdateAsync(project.Id, project);
        }

        private async Task UpdateTaskStateManager(Core.Models.Task task, Project project, User user)
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Assign worker to this Assignment\n" +
                "2. Close assignment\n" +
                "3. Exit");

            string input = StringValidator.ReadLineOrDefault();
            switch (input)
            {
                case "1":
                    User chosenUser = await _taskUI.GetUserByProjects(project);
                    await _taskUI.ChangeExecutor(task, chosenUser);
                    break;
                case "2":
                    task.Status = Status.Done;
                    await _taskUI.UpdateAsync(task.Id, task);
                    break;
                case "3":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Please enter a valid number");
                    break;
            }
        }

        public async Task ChangeDuty(User user)
        {
            Duty duty = await DutyChooser();

            user.Duty = duty;
            await _userService.Update(user.Id, user);
        }

        private async Task<Duty> DutyChooser()
        {
            Console.WriteLine("Please choose the priority of this assignment:");
            foreach (Duty duty in Enum.GetValues(typeof(Duty)))
            {
                Console.WriteLine($"- {duty}");
            }

            Duty chosenDuty;
            while (true)
            {
                Console.Write("Enter duty: ");
                string input = await Task.Run(StringValidator.ReadLineOrDefault);
                if (Enum.TryParse(input, out chosenDuty))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid duty. Please try again.");
                }
            }

            return chosenDuty;
        }
    }
}
