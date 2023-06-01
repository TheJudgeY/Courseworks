using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace UI.ConsoleManagers
{
    public class TesterUI
    {
        public async Task PerformOperationsAsync(User user)
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
    }
}
