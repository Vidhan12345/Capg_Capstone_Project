using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests.Services
{
    public class LeaveServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LeaveService _service;

        public LeaveServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new LeaveService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ApplyAsync_WithPastDate_ThrowsInvalidOperationException()
        {
            var dto = new ApplyLeaveDto
            {
                FromDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                ToDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)),
                LeaveType = "Casual"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ApplyAsync(1, dto));
            Assert.Equal("Cannot apply leave for past dates", ex.Message);
        }

        [Fact]
        public async Task ApplyAsync_WithOverlappingLeave_ThrowsInvalidOperationException()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dto = new ApplyLeaveDto { FromDate = today, ToDate = today.AddDays(2), LeaveType = "Casual" };
            _unitOfWorkMock.Setup(u => u.Leaves.HasOverlappingLeaveAsync(1, dto.FromDate, dto.ToDate, null)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ApplyAsync(1, dto));
            Assert.Equal("Leave overlaps with an existing leave request", ex.Message);
        }

        [Fact]
        public async Task ApplyAsync_ExceedingMaxDays_ThrowsInvalidOperationException()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dto = new ApplyLeaveDto { FromDate = today, ToDate = today.AddDays(15), LeaveType = "Casual" };
            _unitOfWorkMock.Setup(u => u.Leaves.HasOverlappingLeaveAsync(1, dto.FromDate, dto.ToDate, null)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ApplyAsync(1, dto));
            Assert.Contains("Maximum 12 days allowed", ex.Message);
        }

        [Fact]
        public async Task ApplyAsync_WithValidData_CreatesLeave()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dto = new ApplyLeaveDto { FromDate = today, ToDate = today.AddDays(2), LeaveType = "Casual" };
            _unitOfWorkMock.Setup(u => u.Leaves.HasOverlappingLeaveAsync(1, dto.FromDate, dto.ToDate, null)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<Leave>(dto)).Returns(new Leave());
            _unitOfWorkMock.Setup(u => u.Leaves.AddAsync(It.IsAny<Leave>())).ReturnsAsync(new Leave());
            _mapperMock.Setup(m => m.Map<LeaveDto>(It.IsAny<Leave>())).Returns(new LeaveDto());

            var result = await _service.ApplyAsync(1, dto);

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task ApproveAsync_WithNonExistentLeave_ThrowsKeyNotFoundException()
        {
            _unitOfWorkMock.Setup(u => u.Leaves.GetByIdAsync(99)).ReturnsAsync((Leave?)null);

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ApproveAsync(99, 2));
            Assert.Equal("Leave not found", ex.Message);
        }

        [Fact]
        public async Task ApproveAsync_WithAlreadyReviewedLeave_ThrowsInvalidOperationException()
        {
            var leave = new Leave { LeaveId = 1, EmployeeId = 1, Status = "Approved" };
            _unitOfWorkMock.Setup(u => u.Leaves.GetByIdAsync(1)).ReturnsAsync(leave);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ApproveAsync(1, 2));
            Assert.Equal("Leave is already reviewed", ex.Message);
        }

        [Fact]
        public async Task ApproveAsync_SelfApproval_ThrowsInvalidOperationException()
        {
            var leave = new Leave { LeaveId = 1, EmployeeId = 1, Status = "Pending" };
            _unitOfWorkMock.Setup(u => u.Leaves.GetByIdAsync(1)).ReturnsAsync(leave);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ApproveAsync(1, 1));
            Assert.Equal("Cannot approve your own leave", ex.Message);
        }

        [Fact]
        public async Task ApproveAsync_WithValidData_ApprovesLeave()
        {
            var approver = new Employee { EmployeeId = 2, Role = new Role { Name = "Manager" } };
            var leave = new Leave { LeaveId = 1, EmployeeId = 1, Status = "Pending" };
            _unitOfWorkMock.Setup(u => u.Leaves.GetByIdAsync(1)).ReturnsAsync(leave);
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(2)).ReturnsAsync(approver);
            _mapperMock.Setup(m => m.Map<LeaveDto>(It.IsAny<Leave>())).Returns(new LeaveDto { Status = "Approved" });

            var result = await _service.ApproveAsync(1, 2);

            Assert.Equal("Approved", leave.Status);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task RejectAsync_WithValidData_RejectsLeave()
        {
            var leave = new Leave { LeaveId = 1, EmployeeId = 1, Status = "Pending" };
            _unitOfWorkMock.Setup(u => u.Leaves.GetByIdAsync(1)).ReturnsAsync(leave);
            _mapperMock.Setup(m => m.Map<LeaveDto>(It.IsAny<Leave>())).Returns(new LeaveDto { Status = "Rejected" });

            var result = await _service.RejectAsync(1, 2);

            Assert.Equal("Rejected", leave.Status);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}