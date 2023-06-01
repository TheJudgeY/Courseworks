using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.Interfaces
{
    public interface IAssignmentService : IGenericService<Assignment>
    {
        Task CreateAssignment(string name, string desc, DateTime deadline, List<User> workers, User currentUse);
    }
}
