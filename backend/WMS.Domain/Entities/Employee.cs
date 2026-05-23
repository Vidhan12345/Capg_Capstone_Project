using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public int EmployeeId { get; set; }
        [Required, MaxLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(1)]
        public string? Gender { get; set; }
        [Required]
        public DateTime DateOfJoining { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }
        [MaxLength(100)]
        public string? EmergencyContact { get; set; }
        [MaxLength(500)]
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; } = true;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<Leave> ApprovedLeaves { get; set; } = new List<Leave>();
        public ICollection<EmployeeProjectAllocation> Allocations { get; set; } = new List<EmployeeProjectAllocation>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public UserLogin? UserLogin { get; set; }
    }
}
