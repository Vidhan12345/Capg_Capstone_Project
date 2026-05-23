using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UserLoginRepository : GenericRepository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(WMSDbContext context) : base(context) { }

        public async Task<UserLogin?> GetByUsernameAsync(string username)
        {
            return await _dbSet.Include(u => u.Employee).ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserLogin?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
        }
    }
}
