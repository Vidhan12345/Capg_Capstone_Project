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
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeavesController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("apply")]
        public async Task<ActionResult<ApiResponse<LeaveDto>>> Apply([FromBody] ApplyLeaveDto dto)
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _leaveService.ApplyAsync(employeeId, dto);
            return Ok(ApiResponse<LeaveDto>.Ok(result, "Leave applied successfully"));
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<ApiResponse<LeaveDto>>> Cancel(int id)
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _leaveService.CancelAsync(id, employeeId);
            return Ok(ApiResponse<LeaveDto>.Ok(result, "Leave cancelled"));
        }

        [HttpGet("my")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeaveDto>>>> GetMyLeaves()
        {
            var employeeId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _leaveService.GetMyLeavesAsync(employeeId);
            return Ok(ApiResponse<IEnumerable<LeaveDto>>.Ok(result));
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeaveDto>>>> GetPending()
        {
            var result = await _leaveService.GetPendingLeavesAsync();
            return Ok(ApiResponse<IEnumerable<LeaveDto>>.Ok(result));
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<LeaveDto>>> Approve(int id)
        {
            var approverId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _leaveService.ApproveAsync(id, approverId);
            return Ok(ApiResponse<LeaveDto>.Ok(result, "Leave approved"));
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<LeaveDto>>> Reject(int id)
        {
            var approverId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _leaveService.RejectAsync(id, approverId);
            return Ok(ApiResponse<LeaveDto>.Ok(result, "Leave rejected"));
        }
    }
}
