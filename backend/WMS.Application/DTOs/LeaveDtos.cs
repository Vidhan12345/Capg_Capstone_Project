namespace WMS.Application.DTOs
{
    public class LeaveDto
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovedByName { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }

    public class ApplyLeaveDto
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string LeaveType { get; set; } = "Casual";
        public string? Reason { get; set; }
    }

    public class ReviewLeaveDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
