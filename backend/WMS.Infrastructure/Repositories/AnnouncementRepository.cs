using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(WMSDbContext context) : base(context) { }

        public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Include(a => a.PostedByEmployee)
                .Where(a => a.ExpiresAt == null || a.ExpiresAt > now)
                .OrderByDescending(a => a.PostedAt)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _dbSet.Include(a => a.PostedByEmployee).OrderByDescending(a => a.PostedAt).ToListAsync();
        }
    }
}
