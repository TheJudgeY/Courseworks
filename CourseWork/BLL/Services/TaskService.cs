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

            // Check if directory with name {task.Name} {task.Id} exists
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"{task.Name} {task.Id}");
            if (!Directory.Exists(directoryPath))
            {
                // Create directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);
            }

            // Move file to new directory
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
    }
}
