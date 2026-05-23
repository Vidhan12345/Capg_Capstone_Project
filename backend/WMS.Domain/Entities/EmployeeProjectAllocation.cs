using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class EmployeeProjectAllocation : BaseEntity
    {
        public int AllocationId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        [MaxLength(100)]
        public string? RoleOnProject { get; set; }
        public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReleasedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
