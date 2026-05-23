using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department?> GetByNameAsync(string name);
    }
}
