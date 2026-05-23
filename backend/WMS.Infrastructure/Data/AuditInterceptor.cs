using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChanges(eventData, result);

            var auditEntries = new List<AuditLog>();
            var now = DateTime.UtcNow;
            var userId = GetCurrentUserId();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var entityName = entry.Entity.GetType().Name;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "0";

                var audit = new AuditLog
                {
                    EntityName = entityName,
                    RecordId = primaryKey,
                    CreatedBy = userId,
                    CreatedOn = now
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        audit.Action = "Insert";
                        audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        break;
                    case EntityState.Modified:
                        audit.Action = "Update";
                        audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        break;
                    case EntityState.Deleted:
                        audit.Action = "Delete";
                        audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        break;
                }

                auditEntries.Add(audit);
            }

            if (auditEntries.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditEntries);
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return new ValueTask<InterceptionResult<int>>(SavingChanges(eventData, result));
        }

        private int? GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idClaim, out var id))
                    return id;
            }
            return null;
        }
    }
}