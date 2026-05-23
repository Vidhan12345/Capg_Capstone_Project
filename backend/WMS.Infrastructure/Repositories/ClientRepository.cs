using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(WMSDbContext context) : base(context) { }

        public async Task<Client?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ClientName == name);
        }
    }
}
