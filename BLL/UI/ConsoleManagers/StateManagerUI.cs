﻿using BLL.Abstractions.Interfaces;
using Core.Models;

namespace UI.ConsoleManagers
{
    public class StateManagerUI
    {
        private readonly ProjectUI _projectUI;
        public async Task PerformOperationsAsync(User user)
        {
            Dictionary<string, Func<Task>> actions = new Dictionary<string, Func<Task>>()
            {
                { "1", ListProjects},
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Booking operations:\n" +
                    "1. Display all projects\n" +
                    "2. ");
                //Кто создает проект?

                Console.Write("Enter the operation number: ");
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

        public async Task ListProjects()
        {
            Console.Clear();
            await _projectUI.DisplayAllProjectsAsync();
        }
    }
}