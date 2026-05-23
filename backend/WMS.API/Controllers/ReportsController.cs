using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;

namespace WMS_Project.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("attendance")]
        public async Task<ActionResult<ApiResponse<PagedResult<AttendanceReportDto>>>> GetAttendanceReport(
            [FromQuery] AttendanceReportFilterDto filter)
        {
            var result = await _reportService.GetAttendanceReportAsync(filter);
            return Ok(ApiResponse<PagedResult<AttendanceReportDto>>.Ok(result));
        }

        [HttpGet("leave")]
        public async Task<ActionResult<ApiResponse<PagedResult<LeaveReportDto>>>> GetLeaveReport(
            [FromQuery] LeaveReportFilterDto filter)
        {
            var result = await _reportService.GetLeaveReportAsync(filter);
            return Ok(ApiResponse<PagedResult<LeaveReportDto>>.Ok(result));
        }

        [HttpGet("employee")]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeReportDto>>>> GetEmployeeReport(
            [FromQuery] EmployeeReportFilterDto filter)
        {
            var result = await _reportService.GetEmployeeReportAsync(filter);
            return Ok(ApiResponse<PagedResult<EmployeeReportDto>>.Ok(result));
        }
    }
}