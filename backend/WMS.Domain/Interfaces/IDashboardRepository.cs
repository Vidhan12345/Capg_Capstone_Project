using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardStats> GetStatsAsync();
        Task<IEnumerable<DashboardTrend>> GetAttendanceTrendAsync(int days);
        Task<IEnumerable<DashboardDepartmentDistribution>> GetDepartmentDistributionAsync();
    }

    public class DashboardStats
    {
        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public int PendingLeaves { get; set; }
        public int ActiveProjects { get; set; }
    }

    public class DashboardTrend
    {
        public string Date { get; set; } = string.Empty;
        public int Present { get; set; }
        public int Absent { get; set; }
    }

    public class DashboardDepartmentDistribution
    {
        public string Department { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
