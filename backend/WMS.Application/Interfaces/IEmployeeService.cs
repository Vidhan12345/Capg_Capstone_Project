using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;

namespace WMS.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDto>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null, bool ascending = true, int? roleId = null);
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId);
    }
}
