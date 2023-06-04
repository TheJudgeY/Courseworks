using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Abstractions.Models;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IFileAssignmentService : IGenericService<FileAssignment>
    {
        Task CreateRow(AssignmentServiceModel model);
        Task<List<FileCommentModelService>> GetFileCommentByAssignmentId(int id);
    }
}
