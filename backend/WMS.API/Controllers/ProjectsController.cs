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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProjectDto>>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null, [FromQuery] bool ascending = true)
        {
            var result = await _projectService.GetPagedAsync(page, pageSize, sortBy, ascending);
            return Ok(ApiResponse<PagedResult<ProjectDto>>.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound(ApiResponse<ProjectDto>.Fail("Project not found"));
            return Ok(ApiResponse<ProjectDto>.Ok(project));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> Create([FromBody] CreateProjectDto dto)
        {
            var project = await _projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = project.ProjectId },
                ApiResponse<ProjectDto>.Ok(project, "Project created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var project = await _projectService.UpdateAsync(id, dto);
            return Ok(ApiResponse<ProjectDto>.Ok(project, "Project updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _projectService.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Project deleted successfully"));
        }

        [HttpGet("{id}/allocations")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AllocationDto>>>> GetAllocations(int id)
        {
            var allocations = await _projectService.GetAllocationsByProjectAsync(id);
            return Ok(ApiResponse<IEnumerable<AllocationDto>>.Ok(allocations));
        }
    }
}
