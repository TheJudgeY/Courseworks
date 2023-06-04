using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.Models
{
    public class AssignmentServiceModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstimatedTime { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public List<FileCommentModelService> Files { get; set; }
    }
}
