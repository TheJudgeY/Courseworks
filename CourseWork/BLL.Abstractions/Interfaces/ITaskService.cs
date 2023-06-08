using Task = System.Threading.Tasks.Task;

namespace BLL.Abstractions.Interfaces
{
    public interface ITaskService : IGenericService<Core.Models.Task>
    {
        Task OpenFolder(Core.Models.Task task);
        Task CreateAttachment(string filePath, Core.Models.Task task);
    }
}
