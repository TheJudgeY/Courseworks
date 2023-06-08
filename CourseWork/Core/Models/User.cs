using Core.Enums;
using Core.Models;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHashed { get; set; }
    public List<string> Notifications { get; set; }
    public List<Core.Models.Task> Tasks { get; set; }
}