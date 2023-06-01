using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Duty Duty { get; set; }
        public bool IsBuisy { get; set; }
        private static int _lastAssignedId = 0;

        public User()
        {
            Id = ++_lastAssignedId;
        }
    }
}
