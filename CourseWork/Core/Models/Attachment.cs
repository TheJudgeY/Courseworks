using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
