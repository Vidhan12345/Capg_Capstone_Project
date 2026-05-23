using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee?> GetByEmployeeCodeAsync(string code);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    }
}
