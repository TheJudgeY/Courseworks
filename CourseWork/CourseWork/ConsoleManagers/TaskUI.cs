using BLL.Abstractions.Interfaces;
using Core.Models;
using UI.Interfaces;
using Task = System.Threading.Tasks.Task;
using Helpers;
using Core.Enums;
using System.Threading.Tasks;
using Helpers.Validators;

namespace UI.ConsoleManagers
{
    public class TaskUI : ConsoleManager<ITaskService, Core.Models.Task>, IConsoleManager<Core.Models.Task>
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public TaskUI(ITaskService service, IProjectService projectService, IUserService userService) : base(service)
        {
            _projectService = projectService;
            _userService = userService;
        }

        public async Task DisplayTasks(Project project)
        {
            await Console.Out.WriteLineAsync(string.Empty);
            var result = project.Tasks;
            foreach (Core.Models.Task task in result)
            {
                await Console.Out.WriteLineAsync($"================Assignment #{task.Id}=========================\n" +
                                  $"\n{task.Name} -- {task.Status}\n" +
                                  $"\n====================================================================\n" +
                                  $"\nDescription:\n" +
                                  $"\n{StringManipulator.StringWrapper(task.Description)}\n" +
                                  $"    \nEstimated Time: {task.EstimatedTime}\n" +
                                  $"\nAttachments:");
                foreach (Attachment attachment in task.Attachments)
                {
                    await Console.Out.WriteLineAsync($" - {attachment.FileName}");
                }
            }
        }

        public async Task CreateTask(Project project, User user)
        {
            Console.Clear();

            Console.WriteLine("Please enter the name of the assignment:");
            string name = StringValidator.ReadLineOrDefault();

            Console.WriteLine("Please enter description:");
            string description = StringValidator.ReadLineOrDefault();

            DateTime estimatedTime = DateTime.MinValue;
            bool validDate = false;
            while (!validDate)
            {
                Console.WriteLine("Please give an estimated deadline (e.g. 2022-12-31):");
                string input = StringValidator.ReadLineOrDefault();
                if (DateTime.TryParse(input, out estimatedTime))
                {
                    Console.WriteLine("Estimated deadline: " + estimatedTime);
                    validDate = true;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }

            Priority priority = await PriorityChooser();

            Status status = Status.Planned;

            Core.Models.Task task = new Core.Models.Task()
            {
                Name = name,
                Description = description,
                EstimatedTime = estimatedTime,
                Status = status,
                Priority = priority,
                Attachments = new List<Attachment>()
            };   

            await CreateAsync(task);
            user.Tasks.Add(task);
            project.Tasks.Add(task);
            await _userService.Update(user.Id, user);
            await _projectService.Update(project.Id, project);
        }

        private async Task<Priority> PriorityChooser()
        {
            Console.WriteLine("Please choose the priority of this assignment:");
            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
            {
                Console.WriteLine($"- {priority}");
            }

            Priority chosenPriority;
            while (true)
            {
                Console.Write("Enter priority: ");
                string input = await Task.Run(StringValidator.ReadLineOrDefault);
                if (Enum.TryParse(input, out chosenPriority))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid priority. Please try again.");
                }
            }

            return chosenPriority;
        }

        public async Task<Core.Models.Task> ChooseTask(Project project, User user)
        {
            Console.Clear();
            await DisplayTasks(project);

            var tasks = await GetAllAsync();
            Console.WriteLine("Please enter the ID of the project you want to select or 'E' to exit:");
            foreach (Core.Models.Task task in tasks)
            {
                Console.WriteLine($"- {task.Id}: {task.Name}");
            }

            while (true)
            {
                string input = StringValidator.ReadLineOrDefault();

                if (input.ToUpper() == "E")
                {
                    return null;
                }
                else if (!int.TryParse(input, out int taskId))
                {
                    Console.WriteLine("Invalid input: please enter a valid project ID or 'E' to exit.");
                }
                else if (!tasks.Any(task => task.Id == taskId))
                {
                    Console.WriteLine("Invalid input: no project found with that ID.");
                }
                else
                {
                    return await GetByIdAsync(taskId);
                }
            }
        }

        public async Task ChangeExecutor(Core.Models.Task task, User user)
        {
            Project? project = await _projectService.GetProjectByTask(task);
            if (project != null)
            {
                switch (user.Duty)
                {
                    case Duty.Developer:
                        task.Status = Status.Developing;
                        break;
                    case Duty.Tester:
                        task.Status = Status.Testing;
                        break;
                    case Duty.StateManager:
                        if (task.Status == Status.Testing)
                        {
                            task.Status = Status.Revision;
                        }
                        task.Status = Status.Planned;
                        break;
                }

                user.Notifications.Add($"\n - You've been assigned for {task.Name} in {project.Name}");
                await _userService.Update(user.Id, user);
                await UpdateAsync(task.Id, task);
            }
        }

        public async Task AddFile(Core.Models.Task task)
        {
            Console.WriteLine("Please enter the file path:");
            string filePath = StringValidator.ReadLineOrDefault();

            await Service.CreateAttachment(filePath, task);
        }

        public async Task OpenFileAttachments(Core.Models.Task task)
        {
            await Service.OpenFolder(task);
        }

        public async Task<User> GetUserByProjects(Project project)
        {
            Console.Clear();
            Console.WriteLine("Please choose a worker:");
            foreach (User user in project.Workers)
            {
                Console.WriteLine($" - {user.Id}: {user.FirstName} {user.LastName} || {user.Duty}");
            }

            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int userId))
            {
                Console.WriteLine("Invalid input: please enter a valid ID.");
            }
            else if (!project.Workers.Any(user => user.Id == userId))
            {
                Console.WriteLine("Invalid input: no user found with that ID.");
            }
            else
            {
                User chosenUser = await _userService.GetById(userId);
                return chosenUser;
            }

            Console.WriteLine("Something went wrong.");
            return null;
        }

        public async Task<User> GetUserAsync()
        {
            Console.Clear();
            var users = await _userService.GetAll();

            Console.WriteLine("Please choose a worker:");
            foreach (User user in users)
            {
                Console.WriteLine($" - {user.Id}: {user.FirstName} {user.LastName}");
            }

            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int userId))
            {
                Console.WriteLine("Invalid input: please enter a valid ID.");
            }
            else if (!users.Any(user => user.Id == userId))
            {
                Console.WriteLine("Invalid input: no user found with that ID.");
            }
            else
            {
                User chosenUser = await _userService.GetById(userId);
                return chosenUser;
            }

            Console.WriteLine("Something went wrong.");
            return null;
        }
    }
}
