using Core.Enums;

namespace Core.Models
{
    public class Assignment : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstimatedTime { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
