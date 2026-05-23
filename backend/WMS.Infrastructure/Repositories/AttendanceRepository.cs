using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(WMSDbContext context) : base(context) { }

        public async Task<Attendance?> GetTodayAttendanceAsync(int employeeId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _dbSet.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);
        }

        public async Task<IEnumerable<Attendance>> GetByEmployeeAsync(int employeeId, DateOnly from, DateOnly to)
        {
            return await _dbSet
                .Where(a => a.EmployeeId == employeeId && a.Date >= from && a.Date <= to)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByDateRangeAsync(DateOnly from, DateOnly to)
        {
            return await _dbSet
                .Include(a => a.Employee).ThenInclude(e => e.Department)
                .Where(a => a.Date >= from && a.Date <= to)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
    }
}
