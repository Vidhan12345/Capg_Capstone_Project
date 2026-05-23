namespace WMS.Application.DTOs
{
    public class AttendanceReportDto
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Department { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal TotalHours { get; set; }
        public string WorkMode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class AttendanceReportFilterDto
    {
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? Status { get; set; }
        public string? WorkMode { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class LeaveReportDto
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Department { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int DaysRequested { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }

    public class LeaveReportFilterDto
    {
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? LeaveType { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class EmployeeReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? DepartmentName { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TotalProjects { get; set; }
        public int TotalLeaves { get; set; }
        public int TotalAttendance { get; set; }
    }

    public class EmployeeReportFilterDto
    {
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}