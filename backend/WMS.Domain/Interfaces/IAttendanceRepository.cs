using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<Attendance?> GetTodayAttendanceAsync(int employeeId);
        Task<IEnumerable<Attendance>> GetByEmployeeAsync(int employeeId, DateOnly from, DateOnly to);
        Task<IEnumerable<Attendance>> GetByDateRangeAsync(DateOnly from, DateOnly to);
    }
}
