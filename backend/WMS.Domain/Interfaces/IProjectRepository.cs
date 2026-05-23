using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetByClientAsync(int clientId);
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
        Task<IEnumerable<EmployeeProjectAllocation>> GetAllocationsByProjectAsync(int projectId);
        Task<IEnumerable<EmployeeProjectAllocation>> GetAllocationsByEmployeeAsync(int employeeId);
        Task<bool> IsEmployeeAllocatedAsync(int employeeId, int projectId);
        Task<EmployeeProjectAllocation> AddAllocationAsync(EmployeeProjectAllocation allocation);
        Task<EmployeeProjectAllocation?> GetAllocationByIdAsync(int allocationId);
    }
}
