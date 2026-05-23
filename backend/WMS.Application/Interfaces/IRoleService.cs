using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
    }
}
