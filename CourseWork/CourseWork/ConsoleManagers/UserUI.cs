using Core.Models;
using BLL.Abstractions.Interfaces;
using UI.Interfaces;
using Task = System.Threading.Tasks.Task;
using Helpers.Validators;
using Helpers;
using Core.Enums;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace UI.ConsoleManagers
{
    public class UserUI : ConsoleManager<IUserService, User>, IConsoleManager<User>
    {
        private readonly DutyPermission _dutyPermission;
        private readonly ProjectUI _projectUI;
        public UserUI(IUserService service, DutyPermission dutyPermission, ProjectUI projectUI) : base(service)
        {
            _dutyPermission = dutyPermission;
            _projectUI = projectUI;
        }

        public async Task PerformOperationsAsync()
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>
            {
                { "1", SignUpAsync },
                { "2", SignInAsync },
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Sign Up");
                Console.WriteLine("2. Sign In");
                Console.WriteLine("3. Exit");

                Console.Write("Enter the operation number: ");
                string input = StringValidator.ReadLineOrDefault();

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

            Console.WriteLine("Please enter your First Name:");
            string firstName = StringValidator.ReadLineOrDefault();

            Console.WriteLine("Please enter your Last Name:");
            string lastName = StringValidator.ReadLineOrDefault();

            string email = "";
            while (!StringValidator.isValidEmail(email))
            {
                Console.WriteLine("Please enter your email:");
                email = StringValidator.ReadLineOrDefault();
            }

            Console.WriteLine("Please create a password:");
            string password = StringValidator.ReadLineOrDefault();

            User user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                PasswordHashed = PasswordHasher.HashPassword(password),
                Email = email,
                Duty = Duty.Unassigned,
                Notifications = new List<string>(),
                Tasks = new List<Core.Models.Task>()
            };

            var users = await GetAllAsync();
            if (!users.Any())
            {
                await CreateAsync(user);
                await _projectUI.CreateNewProject(user);
                user.Duty = Duty.StateManager;
                await UpdateAsync(user.Id, user);
            }
            else
            {
                await CreateAsync(user);
            }
            await UserMenu(user);
        }

        private async Task SignInAsync()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                string email = "";
                while (!StringValidator.isValidEmail(email))
                {
                    Console.WriteLine("Please enter your email:");
                    email = StringValidator.ReadLineOrDefault();
                }

                Console.WriteLine("Please enter a password:");
                string password = StringValidator.ReadLineOrDefault();

                var result = await GetAllAsync();
                bool authenticated = false;
                foreach (User user in result)
                {
                    if (user.Email == email && await Service.Authenticate(user, password))
                    {
                        await UserMenu(user);
                        authenticated = true;
                        break;
                    }
                }

                if (!authenticated)
                {
                    Console.WriteLine("Invalid username or password\n" +
                        "Would you like to try again? (Y/N)");
                    string input = StringValidator.ReadLineOrDefault();
                    if (input.ToUpper() == "N")
                    {
                        exit = true;
                    }
                }
            }
        }

        private async Task UserMenu(User user)
        {
            await SeeNotifications(user);

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Please choose one of the following options:\n" +
                "1. Log Out\n" +
                "2. Choose Project\n" +
                "3. See Notifications");

                string input = StringValidator.ReadLineOrDefault();
                switch (input)
                {
                    case "1":
                        await PerformOperationsAsync();
                        break;
                    case "2":
                        await ChooseProject(user);
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

        private async Task ChooseProject(User user)
        {
            bool exit = false;
            while (!exit)
            {
                var project = await _projectUI.GetProjectAsync();
                if (project == null)
                {
                    exit = true;
                    continue;
                }

                if (project.Workers.Any(w => w.Id == user.Id))
                {
                    await _dutyPermission.DutyIdentifier(user, project);
                }
                else
                {
                    Console.WriteLine("You are currently not assigned to this project.");
                    Console.WriteLine("Would you like to rechoose the project or exit? (Enter 'R' to rechoose or 'E' to exit)");
                    string input = StringValidator.ReadLineOrDefault();
                    if (input.ToUpper() == "E")
                    {
                        exit = true;
                    }
                }
            }
        }

        private async Task SeeNotifications(User user)
        {
            if (user.Notifications.Any())
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
    }
}
