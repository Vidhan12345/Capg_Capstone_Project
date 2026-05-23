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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats()
        {
            var stats = await _dashboardService.GetStatsAsync();
            return Ok(ApiResponse<DashboardStatsDto>.Ok(stats));
        }

        [HttpGet("attendance-trend")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceTrendDto>>>> GetAttendanceTrend(
            [FromQuery] int days = 7)
        {
            var trend = await _dashboardService.GetAttendanceTrendAsync(days);
            return Ok(ApiResponse<IEnumerable<AttendanceTrendDto>>.Ok(trend));
        }

        [HttpGet("department-distribution")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDistributionDto>>>> GetDepartmentDistribution()
        {
            var dist = await _dashboardService.GetDepartmentDistributionAsync();
            return Ok(ApiResponse<IEnumerable<DepartmentDistributionDto>>.Ok(dist));
        }
    }
}
