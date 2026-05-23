namespace WMS.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public int PendingLeaves { get; set; }
        public int ActiveProjects { get; set; }
    }

    public class AttendanceTrendDto
    {
        public string Date { get; set; } = string.Empty;
        public int Present { get; set; }
        public int Absent { get; set; }
    }

    public class DepartmentDistributionDto
    {
        public string Department { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
