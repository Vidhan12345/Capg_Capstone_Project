namespace WMS.Application.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
