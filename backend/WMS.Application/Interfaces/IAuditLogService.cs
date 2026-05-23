using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;

namespace WMS.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task<PagedResult<AuditLogDto>> GetPagedAsync(AuditLogFilterDto filter);
        Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string entityName, string recordId);
    }
}