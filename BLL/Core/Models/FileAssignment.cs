using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class FileAssignment : BaseEntity
    {
        public int AssignmentId { get; set; }
        public string Comment { get; set; }
        public string FilePath { get; set; }
    }
}
