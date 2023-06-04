using Core.Models;

namespace BLL.Abstractions.Models
{
    public class ProjectServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserServiceModel> Users { get; set; }
        public List<AssignmentServiceModel> Assignments { get; set; }
    }
} 
