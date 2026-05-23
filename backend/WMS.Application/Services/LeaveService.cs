using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LeaveDto> ApplyAsync(int employeeId, ApplyLeaveDto dto)
        {
            if (dto.FromDate > dto.ToDate)
                throw new InvalidOperationException("FromDate must be before or equal to ToDate");

            if (dto.FromDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new InvalidOperationException("Cannot apply leave for past dates");

            var overlapping = await _unitOfWork.Leaves.HasOverlappingLeaveAsync(employeeId, dto.FromDate, dto.ToDate);
            if (overlapping)
                throw new InvalidOperationException("Leave overlaps with an existing leave request");

            var maxAllowed = dto.LeaveType == "Sick" ? 10 : dto.LeaveType == "Earned" ? 20 : 12;
            var daysRequested = (dto.ToDate.DayNumber - dto.FromDate.DayNumber) + 1;
            if (daysRequested > maxAllowed)
                throw new InvalidOperationException($"Maximum {maxAllowed} days allowed for {dto.LeaveType} leave");

            var leave = _mapper.Map<Leave>(dto);
            leave.EmployeeId = employeeId;
            leave.Status = "Pending";
            leave.AppliedAt = DateTime.UtcNow;

            await _unitOfWork.Leaves.AddAsync(leave);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LeaveDto>(leave);
        }

        public async Task<LeaveDto> CancelAsync(int leaveId, int employeeId)
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
            if (leave == null) throw new KeyNotFoundException("Leave not found");
            if (leave.EmployeeId != employeeId) throw new UnauthorizedAccessException("Cannot cancel another employee's leave");
            if (leave.Status != "Pending") throw new InvalidOperationException("Only pending leaves can be cancelled");

            leave.Status = "Cancelled";
            leave.ReviewedAt = DateTime.UtcNow;

            await _unitOfWork.Leaves.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LeaveDto>(leave);
        }

        public async Task<LeaveDto> ApproveAsync(int leaveId, int approverId)
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
            if (leave == null) throw new KeyNotFoundException("Leave not found");
            if (leave.Status != "Pending") throw new InvalidOperationException("Leave is already reviewed");
            if (leave.EmployeeId == approverId) throw new InvalidOperationException("Cannot approve your own leave");

            var approver = await _unitOfWork.Employees.GetByIdAsync(approverId);
            if (approver == null || (approver.Role?.Name != "Manager" && approver.Role?.Name != "Admin"))
                throw new UnauthorizedAccessException("Only managers or admins can approve leaves");

            leave.Status = "Approved";
            leave.ApprovedBy = approverId;
            leave.ReviewedAt = DateTime.UtcNow;

            await _unitOfWork.Leaves.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LeaveDto>(leave);
        }

        public async Task<LeaveDto> RejectAsync(int leaveId, int approverId)
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
            if (leave == null) throw new KeyNotFoundException("Leave not found");
            if (leave.Status != "Pending") throw new InvalidOperationException("Leave is already reviewed");

            leave.Status = "Rejected";
            leave.ApprovedBy = approverId;
            leave.ReviewedAt = DateTime.UtcNow;

            await _unitOfWork.Leaves.UpdateAsync(leave);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LeaveDto>(leave);
        }

        public async Task<IEnumerable<LeaveDto>> GetMyLeavesAsync(int employeeId)
        {
            var leaves = await _unitOfWork.Leaves.GetByEmployeeAsync(employeeId);
            return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
        }

        public async Task<IEnumerable<LeaveDto>> GetPendingLeavesAsync()
        {
            var leaves = await _unitOfWork.Leaves.GetPendingLeavesAsync();
            return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
        }
    }
}
