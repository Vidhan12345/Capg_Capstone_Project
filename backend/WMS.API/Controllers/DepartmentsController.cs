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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(departments));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound(ApiResponse<DepartmentDto>.Fail("Department not found"));
            return Ok(ApiResponse<DepartmentDto>.Ok(department));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> Create([FromBody] CreateDepartmentDto dto)
        {
            var department = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = department.DepartmentId },
                ApiResponse<DepartmentDto>.Ok(department, "Department created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            var department = await _departmentService.UpdateAsync(id, dto);
            return Ok(ApiResponse<DepartmentDto>.Ok(department, "Department updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Department deleted successfully"));
        }
    }
}
