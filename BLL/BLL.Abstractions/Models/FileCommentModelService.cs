using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.Models
{
    public class FileCommentModelService
    {
        public string Comment { get; set; }
        public FileInfo File { get; set; }
    }
}
