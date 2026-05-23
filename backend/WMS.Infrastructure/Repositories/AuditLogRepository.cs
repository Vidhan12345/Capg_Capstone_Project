using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(WMSDbContext context) : base(context) { }

        public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, string recordId)
        {
            return await _dbSet
                .Where(a => a.EntityName == entityName && a.RecordId == recordId)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(int? createdBy)
        {
            return await _dbSet
                .Where(a => a.CreatedBy == createdBy)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
        }
    }
}
