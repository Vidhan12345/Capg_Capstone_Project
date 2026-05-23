using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<IEnumerable<AttendanceTrendDto>> GetAttendanceTrendAsync(int days = 7);
        Task<IEnumerable<DepartmentDistributionDto>> GetDepartmentDistributionAsync();
    }
}
