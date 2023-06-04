using Core.Models;
using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class FileAssignmentService : GenericService<FileAssignment>, IFileAssignmentService
    {
        public FileAssignmentService(IStorage<FileAssignment> storage) : base(storage)
        {
        }

        public async Task CreateRow(AssignmentServiceModel model)
        {
            if (model.Files.Any())
            {
                string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Assignment {model.Id}");
                if (!Directory.Exists(destinationFolder))
                {
                    await Task.Run(() => Directory.CreateDirectory(destinationFolder));
                }

                foreach (FileCommentModelService fileComment in model.Files)
                {
                    string destinationPath = Path.Combine(destinationFolder, fileComment.File.Name);
                    if (File.Exists(destinationPath))
                    {
                        continue;
                    }
                    fileComment.File.MoveTo(destinationPath);

                    FileAssignment row = new FileAssignment()
                    {
                        AssignmentId = model.Id,
                        Comment = fileComment.Comment,
                        FilePath = destinationPath
                    };

                    await Add(row);
                }
            }
        }

        public async Task<List<FileCommentModelService>> GetFileCommentByAssignmentId(int id)
        {
            var table = await GetAll();
            List<FileCommentModelService> tableServiceModel = new List<FileCommentModelService>();

            foreach (FileAssignment row in table)
            {
                if(row.AssignmentId == id)
                {
                    FileInfo file = new FileInfo(row.FilePath);

                    FileCommentModelService fileComment = new FileCommentModelService()
                    {
                        File = file,
                        Comment = row.Comment
                    };

                    tableServiceModel.Add(fileComment);
                }
            }

            return new List<FileCommentModelService>();
        }
    }
}
