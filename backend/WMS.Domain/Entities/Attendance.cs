using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeSpan CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal TotalHours { get; set; }
        [Required, MaxLength(20)]
        public string WorkMode { get; set; } = "WFO";
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Present";
    }
}
