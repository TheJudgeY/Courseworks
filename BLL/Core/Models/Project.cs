using Core.Enums;
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
        public string Description { get; set; }
        public Status DevelopingStatus { get; set; }
        public List<User> AssignedUsers { get; set; }
        private static int _lastAssignedId = 0;

        public Project()
        {
            Id = ++_lastAssignedId;
        }
    }
}
