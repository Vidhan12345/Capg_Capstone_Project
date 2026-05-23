using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class AuditLog
    {
        public int AuditId { get; set; }
        [Required, MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string RecordId { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string Action { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public int? CreatedBy { get; set; }
        public Employee? CreatedByEmployee { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
