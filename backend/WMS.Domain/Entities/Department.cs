using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Department : BaseEntity
    {
        public int DepartmentId { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
