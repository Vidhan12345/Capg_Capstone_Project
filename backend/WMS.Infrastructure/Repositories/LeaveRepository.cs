using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class LeaveRepository : GenericRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(WMSDbContext context) : base(context) { }

        public async Task<IEnumerable<Leave>> GetByEmployeeAsync(int employeeId)
        {
            return await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Approver)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Leave>> GetPendingLeavesAsync()
        {
            return await _dbSet
                .Include(l => l.Employee).ThenInclude(e => e.Department)
                .Include(l => l.Approver)
                .Where(l => l.Status == "Pending")
                .OrderBy(l => l.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Leave>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(l => l.Employee).ThenInclude(e => e.Department)
                .Include(l => l.Approver)
                .Where(l => l.Status == status)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingLeaveAsync(int employeeId, DateOnly from, DateOnly to, int? excludeLeaveId = null)
        {
            return await _dbSet.AnyAsync(l =>
                l.EmployeeId == employeeId &&
                l.Status != "Rejected" &&
                l.FromDate <= to && l.ToDate >= from &&
                (!excludeLeaveId.HasValue || l.LeaveId != excludeLeaveId.Value));
        }

        public override async Task<Leave?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(l => l.Employee)
                .Include(l => l.Approver)
                .FirstOrDefaultAsync(l => l.LeaveId == id);
        }
    }
}
