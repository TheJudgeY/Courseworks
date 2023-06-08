using Core.Enums;

namespace Core.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public Duty Duty { get; set; }
        public List<string> Notifications { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
