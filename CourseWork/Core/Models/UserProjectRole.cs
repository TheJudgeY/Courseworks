using Core.Enums;

namespace Core.Models
{
    public class UserProjectRole : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public Duty Duty { get; set; }
    }
}
