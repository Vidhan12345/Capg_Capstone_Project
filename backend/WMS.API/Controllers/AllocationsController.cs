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
    public class AllocationsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public AllocationsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("my")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AllocationDto>>>> GetMyAllocations()
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _projectService.GetMyAllocationsAsync(employeeId);
            return Ok(ApiResponse<IEnumerable<AllocationDto>>.Ok(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<AllocationDto>>> Allocate([FromBody] CreateAllocationDto dto)
        {
            var result = await _projectService.AllocateEmployeeAsync(dto);
            return Ok(ApiResponse<AllocationDto>.Ok(result, "Employee allocated successfully"));
        }

        [HttpPut("{id}/release")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<object>>> Release(int id)
        {
            await _projectService.ReleaseEmployeeAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Employee released from project"));
        }
    }
}
