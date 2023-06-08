using Core.Models;
using BLL.Abstractions.Interfaces;
using UI.Interfaces;
using Task = System.Threading.Tasks.Task;
using Helpers.Validators;
using BLL.Services;
using Core.Enums;

namespace UI.ConsoleManagers
{
    public class ProjectUI : ConsoleManager<IProjectService, Project>, IConsoleManager<Project>
    {
        public ProjectUI(IProjectService projectService) : base(projectService)
        {
        }
        public async Task DisplayAllProjectsAsync()
        {
            var result = await GetAllAsync();

            foreach (Project project in result)
            {
                Console.WriteLine($"{project.Id}. {project.Name}\n" +
                    $"-------------Description-------------\n" +
                    $"{project.Description}\n" +
                    $"-------------Assignments-------------\n");
                foreach (Core.Models.Task tasks in project.Tasks)
                {
                    Console.WriteLine($" - {tasks.Name} -- {tasks.Status}");
                }

                Console.WriteLine("\n-----------------------------------\n");
            }
        }

        public async Task<Project> CreateNewProject(User user)
        {
            Console.WriteLine("Please enter the name of the project:");
            string name = StringValidator.ReadLineOrDefault();

            Console.WriteLine("Please enter the description:");
            string description = StringValidator.ReadLineOrDefault();

            List<User> users = new List<User>();
            users.Add(user);

            Project project = new Project()
            {
                Name = name,
                Description = description,
                Workers = users,
                Tasks = new List<Core.Models.Task>()
            };
            await CreateAsync(project);

            return project;
        }

        public async Task<Project> GetProjectAsync()
        {
            Console.Clear();

            await DisplayAllProjectsAsync();

            var projects = await GetAllAsync();
            Console.WriteLine("Please enter the ID of the project you want to select or 'E' to exit:");
            foreach (Project project in projects)
            {
                Console.WriteLine($"- {project.Id}: {project.Name}");
            }

            while (true)
            {
                string input = StringValidator.ReadLineOrDefault();

                if (input.ToUpper() == "E")
                {
                    return null;
                }
                else if (!int.TryParse(input, out int projectId))
                {
                    Console.WriteLine("Invalid input: please enter a valid project ID or 'E' to exit.");
                }
                else if (!projects.Any(project => project.Id == projectId))
                {
                    Console.WriteLine("Invalid input: no project found with that ID.");
                }
                else
                {
                    return await GetByIdAsync(projectId);
                }
            }
        }
    }
}
