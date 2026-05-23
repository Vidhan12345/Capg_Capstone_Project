using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(WMSDbContext context) : base(context) { }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee?> GetByEmployeeCodeAsync(string code)
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).FirstOrDefaultAsync(e => e.EmployeeCode == code);
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).Where(e => e.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).Where(e => e.IsActive).ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbSet.Include(e => e.Department).Include(e => e.Role).ToListAsync();
        }

        public override async Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, System.Linq.Expressions.Expression<Func<Employee, bool>>? filter = null,
            string? sortBy = null, bool ascending = true)
        {
            var query = _dbSet.Include(e => e.Department).Include(e => e.Role).AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(Employee), "x");
                var property = System.Linq.Expressions.Expression.Property(parameter, sortBy);
                var lambda = System.Linq.Expressions.Expression.Lambda(property, parameter);

                var methodName = ascending ? "OrderBy" : "OrderByDescending";
                var resultExpression = System.Linq.Expressions.Expression.Call(typeof(Queryable), methodName,
                    new[] { typeof(Employee), property.Type }, query.Expression, System.Linq.Expressions.Expression.Quote(lambda));
                query = query.Provider.CreateQuery<Employee>(resultExpression);
            }

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }
    }
}
