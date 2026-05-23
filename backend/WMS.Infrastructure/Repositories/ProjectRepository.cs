using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(WMSDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetByClientAsync(int clientId)
        {
            return await _dbSet.Where(p => p.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _dbSet
                .Include(p => p.Client)
                .Where(p => p.Status != "Completed")
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeProjectAllocation>> GetAllocationsByProjectAsync(int projectId)
        {
            return await _context.EmployeeProjectAllocations
                .Include(a => a.Employee)
                .Where(a => a.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeProjectAllocation>> GetAllocationsByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeProjectAllocations
                .Include(a => a.Project)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeAllocatedAsync(int employeeId, int projectId)
        {
            return await _context.EmployeeProjectAllocations
                .AnyAsync(a => a.EmployeeId == employeeId && a.ProjectId == projectId && a.IsActive);
        }

        public async Task<EmployeeProjectAllocation> AddAllocationAsync(EmployeeProjectAllocation allocation)
        {
            var entry = await _context.EmployeeProjectAllocations.AddAsync(allocation);
            return entry.Entity;
        }

        public async Task<EmployeeProjectAllocation?> GetAllocationByIdAsync(int allocationId)
        {
            return await _context.EmployeeProjectAllocations.FindAsync(allocationId);
        }

        public override async Task<Project?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(p => p.Client).FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Client).ToListAsync();
        }
    }
}
