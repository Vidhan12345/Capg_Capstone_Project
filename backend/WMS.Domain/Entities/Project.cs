using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Project : BaseEntity
    {
        public int ProjectId { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required, MaxLength(20)]
        public string Status { get; set; } = "NotStarted";

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public ICollection<EmployeeProjectAllocation> Allocations { get; set; } = new List<EmployeeProjectAllocation>();
    }
}
