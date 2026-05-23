using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            return client == null ? null : _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto dto)
        {
            var client = _mapper.Map<Client>(dto);
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> UpdateAsync(int id, UpdateClientDto dto)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) throw new KeyNotFoundException("Client not found");

            if (dto.ClientName != null) client.ClientName = dto.ClientName;
            if (dto.ClientAddress != null) client.ClientAddress = dto.ClientAddress;
            if (dto.ClientPhoneNumber != null) client.ClientPhoneNumber = dto.ClientPhoneNumber;
            if (dto.ClientLocation != null) client.ClientLocation = dto.ClientLocation;
            if (dto.Status.HasValue) client.Status = dto.Status.Value;

            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ClientDto>(client);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) throw new KeyNotFoundException("Client not found");

            if (await _unitOfWork.Projects.ExistsAsync(p => p.ClientId == id))
                throw new InvalidOperationException("Cannot delete client with active projects");

            client.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }
    }
}
