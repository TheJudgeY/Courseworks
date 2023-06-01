using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Assignment : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public User ResponsibleUser { get; set; }
        public List<User> Workers { get; set; }
        private static int _lastAssignedId = 0;

        public Assignment()
        {
            Id = ++_lastAssignedId;
        }
    }
}
