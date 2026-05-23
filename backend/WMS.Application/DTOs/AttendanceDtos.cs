namespace WMS.Application.DTOs
{
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal TotalHours { get; set; }
        public string WorkMode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CheckInDto
    {
        public TimeSpan CheckIn { get; set; }
        public string WorkMode { get; set; } = "WFO";
    }

    public class CheckOutDto
    {
        public TimeSpan CheckOut { get; set; }
    }
}
