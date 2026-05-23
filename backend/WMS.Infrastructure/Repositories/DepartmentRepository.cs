using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(WMSDbContext context) : base(context) { }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Name == name);
        }
    }
}
