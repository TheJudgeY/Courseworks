using BLL.Abstractions.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Models;
using Helpers;
using UI.Interfaces;
using Core.Enums;
using Helpers.Validators;
using BLL.Services;

namespace UI.ConsoleManagers
{
    public class AssignmentUI : ConsoleManager<IAssignmentService, Assignment>, IConsoleManager<Assignment>
    {
        private readonly IUserService _userService;
        public AssignmentUI(IAssignmentService service, IUserService userService) : base(service)
        {
            _userService = userService;
        }

        public async Task<AssignmentServiceModel> ChooseAssignment(ProjectServiceModel project, UserServiceModel user)
        {
            Console.Clear();
            var result = project.Assignments;
            foreach (AssignmentServiceModel assignment in result)
            {
                Console.WriteLine($"================Assignment #{assignment.Id}=========================\n" +
                                  $"\n{assignment.Name} -- {assignment.Status}\n" +
                                  $"\n====================================================================\n" +
                                  $"\nDescription:\n" +
                                  $"\n{StringManipulator.StringWrapper(assignment.Description)}\n" +
                                  $"    \nEstimated Time: {assignment.EstimatedTime}\n" +
                                  $"\nAttachments:");
                foreach (FileCommentModelService filecomment in assignment.Files)
                {
                    Console.WriteLine($" - {filecomment.File.Name}");
                }
            }

            Console.WriteLine("\nPlease enter the ID of the assignment you want to select:\n");
            foreach (AssignmentServiceModel assignment in result)
            {
                Console.WriteLine($"- {assignment.Id}: {assignment.Name}");
            }

            while (true)
            {
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int assignmentId))
                {
                    Console.WriteLine("Invalid input: please enter a valid ID.");
                }
                else if (!result.Any(assignment => assignment.Id == assignmentId))
                {
                    Console.WriteLine("Invalid input: no assignment found with that ID.");
                }
                else
                {
                    var assignment = await Service.GetAssignmentById(assignmentId, project.Id);
                    return await CheckAssignment(assignment, user);
                }
            }
        }

        private async Task<AssignmentServiceModel> CheckAssignment(AssignmentServiceModel assignment, UserServiceModel user)
        {
            if(await Service.CheckAvailibilityForUser(user, assignment))
            {
                return assignment;
            }

            return null;
        }

        public async Task<AssignmentServiceModel> CreateAssignment(UserServiceModel user, ProjectServiceModel project) 
        { 
            Console.WriteLine("Please enter the name of the assignment:");
            string? name = Console.ReadLine();

            Console.WriteLine("Please enter description:");
            string? description = Console.ReadLine();

            Console.WriteLine("Please give an estimated deadline (e.g. 2022-12-31):");
            string input = Console.ReadLine();
            DateTime estimatedTime;
            if (DateTime.TryParse(input, out estimatedTime))
            {
                Console.WriteLine("Estimated deadline: " + estimatedTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }

            Priority priority = await PriorityChooser();

            Status status = Status.Planned;

            AssignmentServiceModel assignment = new AssignmentServiceModel()
            {
                Name = name,
                Description = description,
                EstimatedTime = estimatedTime,
                Priority = priority,
                Status = status,
                Files = new List<FileCommentModelService>()
            };

            user.Assignments.Add(assignment);
            project.Assignments.Add(assignment);
            await Service.CreateAssignment(assignment, project.Id, user.Id);

            return assignment;
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
                string input = await Task.Run(Console.ReadLine);
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

        public async Task AddFile(AssignmentServiceModel assignment)
        {
            Console.Write("Enter file path: ");
            string filePath = Console.ReadLine();
            if (!StringValidator.IsValidFilePath(filePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            FileInfo file = new FileInfo(filePath);
            Console.WriteLine("Please add comment:");
            string? comment = Console.ReadLine();

            FileCommentModelService fileComment = new FileCommentModelService()
            {
                Comment = comment,
                File = file
            };
            assignment.Files.Add(fileComment);

            await Service.UpdateAssignment(assignment);
        }

        public async Task ChangeExecutor(AssignmentServiceModel assignment, ProjectServiceModel project)
        {
            Console.WriteLine("Who do you want to transfer this assignment to?\n");
            foreach (UserServiceModel user in project.Users)
            {
                Duty duty = await _userService.GetUserDutyByIds(user.Id, project.Id);
                Console.WriteLine($" - {user.Id}: {user.FirstName} {user.LastName} || {duty}");
            }

            bool exit = false;
            while (!exit)
            {
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
                    Duty duty = await _userService.GetUserDutyByIds(chosenUser.Id, project.Id);
                    switch (duty)
                    {
                        case Duty.Developer:
                            assignment.Status = Status.Developing;
                            break;
                        case Duty.Tester:
                            assignment.Status = Status.Testing;
                            break;
                        case Duty.StateManager:
                            if (assignment.Status == Status.Testing)
                            {
                                assignment.Status = Status.Revision;
                            }
                            assignment.Status = Status.Planned;
                            break;
                    }

                    chosenUser.Notifications.Add($" - You've been assigned for {assignment.Name} in {project.Name}");
                    chosenUser.Assignments.Add(assignment);
                    await Service.UpdateUserAssignment(assignment, chosenUser);
                    exit = true;
                }
            }
        }


        public async Task UpdateAssignmentStateManager(AssignmentServiceModel assignment, ProjectServiceModel project)
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                "1. Assign worker to this Assignment\n" +
                "2. Close assignment\n" +
                "3. Exit");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    await ChangeExecutor(assignment, project);
                    break;
                case "2":
                    assignment.Status = Status.Done;
                    break;
                case "3":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Please enter a valid number");
                    break;
            }

            await Service.UpdateAssignment(assignment);
        }
    }
}
