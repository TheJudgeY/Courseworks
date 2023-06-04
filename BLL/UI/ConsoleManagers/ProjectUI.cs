using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Models;
using Core.Enums;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class ProjectUI : ConsoleManager<IProjectService, Project>, IConsoleManager<Project>
    {
        private readonly IUserService _userService;
        private readonly IUserProjectRoleService _userProjectRoleService;
        public ProjectUI(IProjectService service, IUserService userService, IUserProjectRoleService userProjectRoleService) : base(service)
        {
            _userService = userService;
            _userProjectRoleService = userProjectRoleService;
        }

        public async Task DisplayAllProjectsAsync()
        {
            var result = await Service.GetProjects();

            foreach(ProjectServiceModel project in result) 
            {
                Console.WriteLine($"{project.Id}. {project.Name}\n" +
                    $"-------------Description-------------\n" +
                    $"{project.Description}\n" +
                    $"-------------Assignments-------------\n");
                foreach(AssignmentServiceModel assignment in project.Assignments)
                {
                    Console.WriteLine($" - {assignment.Name} -- {assignment.Status}");
                }

                Console.WriteLine("\n-----------------------------------\n");
            }
        }

        public async Task CreateNewProject(UserServiceModel user)
        {
            Console.WriteLine("Please enter the name of the project:");
            string? name = Console.ReadLine();

            Console.WriteLine("Please enter the description:");
            string? description = Console.ReadLine();

            List<UserServiceModel> users = new List<UserServiceModel>();
            users.Add(user);
            
            ProjectServiceModel project = new ProjectServiceModel()
            { 
                Name = name,
                Description = description,
                Users = users,
                Assignments = new List<AssignmentServiceModel>()
            };

            project = await Service.CreateProject(project);

            await _userProjectRoleService.SetUserRole(project, user, Duty.StateManager);
        }


        public async Task<ProjectServiceModel> ChooseProject()
        {
            Console.Clear();
            int id = await GetProjectIdAsync();
            var result = await Service.GetProjectById(id);
            if (result != null)
            {
                return result;
            }
            Console.WriteLine("An error occured.");
            return null;
        }


        public async Task<int> GetProjectIdAsync()
        {
            Console.Clear();
            await DisplayAllProjectsAsync();

            var projects = await Service.GetProjects();
            Console.WriteLine("Please enter the ID of the project you want to select or 'E' to exit:");
            foreach (ProjectServiceModel project in projects)
            {
                Console.WriteLine($"- {project.Id}: {project.Name}");
            }

            while (true)
            {
                string? input = Console.ReadLine();

                if (input.ToUpper() == "E")
                {
                    return -1;
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
                    return projectId;
                }
            }
        }

        public async Task UpdateProject(ProjectServiceModel project)
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Edit Project Name\n" +
                "2. Edit Project Description\n" +
                "3. Edit Workers\n" +
                "4. Exit");

            string? input = Console.ReadLine();
            switch(input)
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
                    await UpdateProjectWorkers(project);
                    break;
                case "4":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Please enter a valid number");
                    break;
            }

            await Service.UpdateProject(project);
        }

        public async Task UpdateProjectWorkers(ProjectServiceModel project)
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
                    UserServiceModel userRemove = await GetUserFromInteractionAssignment(project);
                    if (userRemove != null)
                    {
                        project.Users.Remove(userRemove);
                    }
                    Console.WriteLine("Something went wrong. No user removed");
                    break;
                case "2":
                    UserServiceModel userAdd = await GetUserFromInteractionProject();
                    if (userAdd != null)
                    {
                        project.Users.Add(userAdd);
                    }
                    break;
                case "3":
                    UserServiceModel worker = await GetUserFromInteractionAssignment(project);
                    await ChangeDuty(worker, project);
                    break;
                case "4":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Please enter a valid number");
                    break;
            }

            await Service.UpdateProject(project);
        }

        public async Task<UserServiceModel> GetUserFromInteractionProject()
        {
            Console.Clear();
            var users = await _userService.GetUsers();

            Console.WriteLine("Please choose a worker:");
            foreach (UserServiceModel user in users)
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
                UserServiceModel chosenUser = await _userService.GetUserById(userId);
                return chosenUser;
            }

            Console.WriteLine("Something went wrong.");
            return null;
        }

        public async Task ChangeDuty(UserServiceModel user, ProjectServiceModel project)
        {
            Duty duty = await DutyChooser();
            await _userProjectRoleService.SetUserRole(project, user, duty);
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
                string input = await Task.Run(Console.ReadLine);
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

        public async Task<UserServiceModel> GetUserFromInteractionAssignment(ProjectServiceModel project)
        {
            Console.Clear();
            Console.WriteLine("Please choose a worker:");
            foreach (UserServiceModel user in project.Users)
            {
                Duty duty = await _userService.GetUserDutyByIds(user.Id, project.Id);
                Console.WriteLine($" - {user.Id}: {user.FirstName} {user.LastName} || {duty}");
            }

            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int userId))
            {
                Console.WriteLine("Invalid input: please enter a valid ID.");
            }
            else if (!project.Users.Any(user => user.Id == userId))
            {
                Console.WriteLine("Invalid input: no user found with that ID.");
            }
            else
            {
                UserServiceModel chosenUser = await _userService.GetUserById(userId);
                return chosenUser; 
            }

            Console.WriteLine("Something went wrong.");
            return null;
        }
    }
}
