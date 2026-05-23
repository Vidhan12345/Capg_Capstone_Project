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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeDto>>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null, [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true, [FromQuery] int? roleId = null)
        {
            var result = await _employeeService.GetPagedAsync(page, pageSize, search, sortBy, ascending, roleId);
            return Ok(ApiResponse<PagedResult<EmployeeDto>>.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound(ApiResponse<EmployeeDto>.Fail("Employee not found"));
            return Ok(ApiResponse<EmployeeDto>.Ok(employee));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> Create([FromBody] CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = employee.EmployeeId },
                ApiResponse<EmployeeDto>.Ok(employee, "Employee created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var employee = await _employeeService.UpdateAsync(id, dto);
            return Ok(ApiResponse<EmployeeDto>.Ok(employee, "Employee updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Employee deleted successfully"));
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetByDepartmentAsync(departmentId);
            return Ok(ApiResponse<IEnumerable<EmployeeDto>>.Ok(employees));
        }
    }
}
