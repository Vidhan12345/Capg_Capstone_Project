using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IAnnouncementRepository : IGenericRepository<Announcement>
    {
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
    }
}
