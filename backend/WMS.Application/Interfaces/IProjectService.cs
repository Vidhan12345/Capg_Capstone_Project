using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;

namespace WMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectDto>> GetPagedAsync(int page, int pageSize, string? sortBy = null, bool ascending = true);
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<AllocationDto>> GetAllocationsByProjectAsync(int projectId);
        Task<IEnumerable<AllocationDto>> GetMyAllocationsAsync(int employeeId);
        Task<AllocationDto> AllocateEmployeeAsync(CreateAllocationDto dto);
        Task ReleaseEmployeeAsync(int allocationId);
    }
}
