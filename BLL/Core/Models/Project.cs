using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public List<User> AvailibleUsers { get; set; }
        public List<User> AssignedUsers { get; set; }
    }
}
