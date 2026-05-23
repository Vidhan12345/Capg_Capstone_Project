using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
        Task<ClientDto> CreateAsync(CreateClientDto dto);
        Task<ClientDto> UpdateAsync(int id, UpdateClientDto dto);
        Task DeleteAsync(int id);
    }
}
