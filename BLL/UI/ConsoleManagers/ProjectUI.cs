using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core.Enums;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public class ProjectUI : ConsoleManager<IProjectService, Project>, IConsoleManager<Project>
    {
        public ProjectUI(IProjectService service) : base(service)
        {
        }

        public async Task DisplayAllProjectsAsync()
        {
            var result = await GetAllAsync();

            foreach(Project project in result) 
            {
                Console.WriteLine($"{project.Id}. {project.Name}\n" +
                    $"-------------Description-------------\n" +
                    $"{project.Description}");
            }
        }

        public async Task MoveToDeveloping()
        {
            await DisplayAllProjectsAsync();

            int projectId = await GetIdAsync();
            var project = await Service.GetById(projectId);

            project.DevelopingStatus = Status.Developing;
        }

        public async Task<int> GetIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
