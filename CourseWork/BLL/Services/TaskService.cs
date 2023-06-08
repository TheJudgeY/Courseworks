using Task = System.Threading.Tasks.Task;
using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;
using System.Diagnostics;

namespace BLL.Services
{
    public class TaskService : GenericService<Core.Models.Task>, ITaskService
    {
        public TaskService(IStorage<Core.Models.Task> storage) : base(storage)
        {
        }

        public async Task CreateAttachment(string filePath, Core.Models.Task task)
        {
            string fileName = Path.GetFileName(filePath);
            byte[] data = File.ReadAllBytes(filePath);
            long size = data.Length;

            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"{task.Name} {task.Id}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string newFilePath = Path.Combine(directoryPath, fileName);
            File.Move(filePath, newFilePath);

            Attachment attachment = new Attachment
            {
                FileName = fileName,
                Size = size,
                Data = data
            };

            task.Attachments.Add(attachment);
            await Update(task.Id, task);
        }

        public async Task OpenFolder(Core.Models.Task task)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"{task.Name} {task.Id}");
            await Task.Run(() => Process.Start("explorer.exe", directoryPath));
        }

        public async Task<bool> CheckAvailibilityForUser(User user, Core.Models.Task task)
        {
            if (user.Tasks.Any(t => t.Id == task.Id))
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
