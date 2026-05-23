using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, string entityId);
        Task<IEnumerable<AuditLog>> GetByUserAsync(int? createdBy);
    }
}
