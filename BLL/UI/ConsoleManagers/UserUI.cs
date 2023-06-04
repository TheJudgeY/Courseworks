using BLL.Abstractions.Interfaces;
using UI.Interfaces;
using Helpers.Validators;
using Core.Enums;
using Core.Models;
using Helpers;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using BLL.Abstractions.Models;

namespace UI.ConsoleManagers
{
    public class UserUI : ConsoleManager<IUserService, User>, IConsoleManager<User>
    {
        private readonly ProjectUI _projectUI;
        private readonly IUserProjectRoleService _userProjectRoleService;
        private readonly DutyPermission _dutyPermission;

        public UserUI(IUserService service, ProjectUI projectUI, IUserProjectRoleService userProjectRoleService, DutyPermission dutyPermission) : base(service)
        {
            _projectUI = projectUI;
            _userProjectRoleService = userProjectRoleService;
            _dutyPermission = dutyPermission;
        }

        public async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
    {
        { "1", SignUpAsync },
        { "2", LogIn }
    };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please choose one of the following options:\n" +
                "1. Sign up\n" +
                "2. Sign in\n" +
                "3. Exit");

                string input = Console.ReadLine();

                if (input == "3")
                {
                    Console.Clear();
                    Environment.Exit(0);
                }

                if (actions.ContainsKey(input))
                {
                    await actions[input]();
                }
                else
                {
                    Console.WriteLine("Invalid operation number.");
                }
            }
        }

        private async Task SignUpAsync()
        {
            
            Console.Clear();
            Console.WriteLine("Please enter your Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Please enter your First Name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Please enter your Last Name:");
            string lastName = Console.ReadLine();

            string email = "";
            while (!StringValidator.isValidEmail(email))
            {
                Console.WriteLine("Please enter your email:");
                email = Console.ReadLine();
            }

            Console.WriteLine("Please create a password:");
            string password = Console.ReadLine();

            UserServiceModel newUserModel = new UserServiceModel()
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                PasswordHashed = PasswordHasher.HashPassword(password),
                Email = email
            };
            newUserModel = await Service.CreateUser(newUserModel);

            var users = await GetAllAsync();
            if (users.Count() == 1)
            {
                await _projectUI.CreateNewProject(newUserModel);
            }
        }

        private async Task LogIn()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Please enter your Username:");
                string username = Console.ReadLine();

                Console.WriteLine("Please enter a password:");
                string password = Console.ReadLine();

                var result = await Service.GetUsers();
                bool authenticated = false;
                foreach (UserServiceModel user in result)
                {
                    if (user.Username == username && await Service.Authenticate(user, password))
                    {
                        await PostLogIn(user);
                        authenticated = true;
                        break;
                    }
                }

                if (!authenticated)
                {
                    Console.WriteLine("Invalid username or password\n" +
                        "Would you like to try again? (Y/N)");
                    string input = Console.ReadLine();
                    if (input.ToUpper() == "N")
                    {
                        exit = true;
                    }
                }
            }
        }

        public async Task PostLogIn(UserServiceModel user)
        {
            Console.Clear();
            await SeeNotifications(user);

            while (true)
            {
                Console.WriteLine("Greetings!\n" +
                "===========================================\n" +
                "Please choose one of the following options:\n" +
                "1. Log Out\n" +
                "2. Choose Project\n" +
                "3. See Notifications");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await PerformOperationsAsync();
                        break;
                    case "2":
                        await ProjectChoosingInteractions(user);
                        break;
                    case "3":
                        await SeeNotifications(user);
                        break;
                    default:
                        Console.WriteLine("Invalid operation number.");
                        break;
                }
            }
        }

        private async Task SeeNotifications(UserServiceModel user)
        {
            if (user.Notifications != null)
            {
                await Console.Out.WriteLineAsync("==================================================\n");
                foreach (string notification in user.Notifications)
                {
                    await Console.Out.WriteLineAsync($"Notification: {notification}");
                }
                await Console.Out.WriteLineAsync("\n================================================");
            }
            else
            {
                await Console.Out.WriteLineAsync("No notifications");
            }
        }

        private async Task ProjectChoosingInteractions(UserServiceModel user)
        {
            bool exit = false;
            while (!exit)
            {
                await _projectUI.DisplayAllProjectsAsync();

                int id = await _projectUI.GetProjectIdAsync();
                if (id == -1)
                {
                    exit = true;
                    continue;
                }

                var table = await _userProjectRoleService.GetTableByProjectId(id);

                if (table != null)
                {
                    foreach (var row in table)
                    {
                        if (row.UserId == user.Id)
                        {
                            await _dutyPermission.DutyIdentifier(row);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You are currently not assigned for that project. Please contact HR if there is an issue.");
                    Console.WriteLine("Would you like to rechoose the project or exit? (Enter 'R' to rechoose or 'E' to exit)");
                    string input = Console.ReadLine();
                    if (input.ToUpper() == "E")
                    {
                        exit = true;
                    }
                }
            }
        }
    }
}
