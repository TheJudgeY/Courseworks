using BLL.Abstractions.Interfaces;
using UI.Interfaces;
using Helpers.Validators;
using Core.Enums;
using Core.Models;
using Helpers;

namespace UI.ConsoleManagers
{
    public class UserUI : ConsoleManager<IUserService, User>, IConsoleManager<User>
    {
        public UserUI(IUserService userService) : base(userService)
        {
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
                Console.WriteLine("Greetings!\n" +
                "===========================================\n" +
                "Please choose one of the following options:\n" +
                "1. Sign up\n" +
                "2. Sign in\n" +
                "3. Exit");
                
                string input = Console.ReadLine();

                if (input == "3")
                {
                    Console.Clear();
                    break;
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

            User newUser = new User()
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = PasswordHasher.HashPassword(password),
                Email = email,
                Duty = Duty.Unassigned,
                IsBuisy = false
            };

            await Service.CreateUser(newUser);
        }
        
        private async Task LogIn() 
        {
            Console.WriteLine("Please enter your Username:");
            string username = Console.ReadLine();

            bool found = false;
            while (!found)
            {
                var result = await Service.GetAll();
                foreach (User user in result)
                {
                    if (user.Username == username)
                    {
                        Console.WriteLine("Please create a password:");
                        string password = Console.ReadLine();
                        found = true;
                        await DutyPermission.DutyIdentifier(user);
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("No matching username was found. Please try again.");
                }
            }
        }
    }
}
