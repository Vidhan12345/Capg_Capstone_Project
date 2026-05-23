using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface ILeaveRepository : IGenericRepository<Leave>
    {
        Task<IEnumerable<Leave>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<Leave>> GetPendingLeavesAsync();
        Task<IEnumerable<Leave>> GetByStatusAsync(string status);
        Task<bool> HasOverlappingLeaveAsync(int employeeId, DateOnly from, DateOnly to, int? excludeLeaveId = null);
    }
}
