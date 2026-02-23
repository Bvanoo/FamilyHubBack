using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public enum MemberStatus
    {
        Pending,
        Accepted,
        Invited,
        Banned
    }

    [Table("GroupMembers")]
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }

        public string Role { get; set; } = "Member";

        public MemberStatus Status { get; set; } = MemberStatus.Pending;

        [NotMapped]
        public bool IsPending => Status == MemberStatus.Pending || Status == MemberStatus.Invited;

        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public bool SyncCalendar { get; set; } = true;
        public bool ShowDetails { get; set; } = false;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        public bool IsAccepted { get; set; } = false;
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; } = null!;
    }
}