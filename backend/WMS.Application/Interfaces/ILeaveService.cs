using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveDto> ApplyAsync(int employeeId, ApplyLeaveDto dto);
        Task<LeaveDto> CancelAsync(int leaveId, int employeeId);
        Task<LeaveDto> ApproveAsync(int leaveId, int approverId);
        Task<LeaveDto> RejectAsync(int leaveId, int approverId);
        Task<IEnumerable<LeaveDto>> GetMyLeavesAsync(int employeeId);
        Task<IEnumerable<LeaveDto>> GetPendingLeavesAsync();
    }
}
