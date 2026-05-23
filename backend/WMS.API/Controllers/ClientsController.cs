using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;

namespace WMS_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientDto>>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ClientDto>>.Ok(clients));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClientDto>>> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound(ApiResponse<ClientDto>.Fail("Client not found"));
            return Ok(ApiResponse<ClientDto>.Ok(client));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ClientDto>>> Create([FromBody] CreateClientDto dto)
        {
            var client = await _clientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = client.ClientId },
                ApiResponse<ClientDto>.Ok(client, "Client created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ClientDto>>> Update(int id, [FromBody] UpdateClientDto dto)
        {
            var client = await _clientService.UpdateAsync(id, dto);
            return Ok(ApiResponse<ClientDto>.Ok(client, "Client updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _clientService.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Client deleted successfully"));
        }
    }
}
