namespace BLL.Abstractions.Models
{
    public class UserServiceModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHashed { get; set; }
        public string Email { get; set; }
        public List<AssignmentServiceModel> Assignments { get; set; }
        public List<string> Notifications { get; set; }
    }
}
