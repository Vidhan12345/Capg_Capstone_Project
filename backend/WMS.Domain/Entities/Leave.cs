using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Leave : BaseEntity
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        [Required]
        public DateOnly FromDate { get; set; }
        [Required]
        public DateOnly ToDate { get; set; }
        [Required, MaxLength(20)]
        public string LeaveType { get; set; } = "Casual";
        [MaxLength(1000)]
        public string? Reason { get; set; }
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";
        public int? ApprovedBy { get; set; }
        public Employee? Approver { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }
}
