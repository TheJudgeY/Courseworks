using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Notification : BaseEntity
    {
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
