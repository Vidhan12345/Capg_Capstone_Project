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
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> CheckIn([FromBody] CheckInDto dto)
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _attendanceService.CheckInAsync(employeeId, dto);
            return Ok(ApiResponse<AttendanceDto>.Ok(result, "Check-in recorded"));
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> CheckOut([FromBody] CheckOutDto dto)
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _attendanceService.CheckOutAsync(employeeId, dto);
            return Ok(ApiResponse<AttendanceDto>.Ok(result, "Check-out recorded"));
        }

        [HttpGet("today")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> GetToday()
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _attendanceService.GetTodayAttendanceAsync(employeeId);
            if (result == null) return Ok(ApiResponse<AttendanceDto>.Ok(null!, "No check-in record for today"));
            return Ok(ApiResponse<AttendanceDto>.Ok(result));
        }

        [HttpGet("my")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetMyAttendance(
            [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _attendanceService.GetByEmployeeAsync(employeeId, from, to);
            return Ok(ApiResponse<IEnumerable<AttendanceDto>>.Ok(result));
        }

        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetByEmployee(
            int employeeId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            var result = await _attendanceService.GetByEmployeeAsync(employeeId, from, to);
            return Ok(ApiResponse<IEnumerable<AttendanceDto>>.Ok(result));
        }
    }
}
