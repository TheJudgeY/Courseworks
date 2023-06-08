using Core.Enums;

namespace Core.Models
{
    public class Task : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstimatedTime { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
