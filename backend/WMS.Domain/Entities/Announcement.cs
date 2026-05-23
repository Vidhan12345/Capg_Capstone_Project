using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Announcement : BaseEntity
    {
        public int AnnouncementId { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public int PostedBy { get; set; }
        public Employee PostedByEmployee { get; set; } = null!;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
