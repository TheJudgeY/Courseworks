using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserProjectRole : BaseEntity
    {
        public Project Project { get; set; }
        public User User { get; set; }
        public Duty Duty { get; set; }
    }
}
