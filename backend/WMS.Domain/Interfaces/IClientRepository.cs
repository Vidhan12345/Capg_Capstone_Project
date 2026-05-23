using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetByNameAsync(string name);
    }
}
