using Microsoft.EntityFrameworkCore;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly WMSDbContext _context;

        public DashboardRepository(WMSDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStats> GetStatsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return new DashboardStats
            {
                TotalEmployees = await _context.Employees.CountAsync(e => !e.IsDeleted && e.IsActive),
                PresentToday = await _context.Attendances.CountAsync(a => a.Date == today && a.Status == "Present"),
                AbsentToday = await _context.Employees.CountAsync(e => e.IsActive && !e.IsDeleted
                    && !_context.Attendances.Any(a => a.EmployeeId == e.EmployeeId && a.Date == today)),
                PendingLeaves = await _context.Leaves.CountAsync(l => l.Status == "Pending"),
                ActiveProjects = await _context.Projects.CountAsync(p => p.Status != "Completed" && !p.IsDeleted)
            };
        }

        public async Task<IEnumerable<DashboardTrend>> GetAttendanceTrendAsync(int days)
        {
            var from = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-days));
            var rawData = await _context.Attendances
                .Where(a => a.Date >= from)
                .GroupBy(a => a.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Present = g.Count(a => a.Status == "Present"),
                    Absent = g.Count(a => a.Status == "Absent")
                })
                .OrderBy(t => t.Date)
                .ToListAsync();

            return rawData.Select(r => new DashboardTrend
            {
                Date = r.Date.ToString("yyyy-MM-dd"),
                Present = r.Present,
                Absent = r.Absent
            });
        }

        public async Task<IEnumerable<DashboardDepartmentDistribution>> GetDepartmentDistributionAsync()
        {
            return await _context.Departments
                .Where(d => !d.IsDeleted)
                .Select(d => new DashboardDepartmentDistribution
                {
                    Department = d.Name,
                    Count = d.Employees.Count(e => e.IsActive && !e.IsDeleted)
                })
                .ToListAsync();
        }
    }
}
