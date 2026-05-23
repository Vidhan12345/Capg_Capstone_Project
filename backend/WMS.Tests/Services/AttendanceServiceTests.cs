using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests.Services
{
    public class AttendanceServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AttendanceService _service;

        public AttendanceServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _service = new AttendanceService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CheckInAsync_WhenAlreadyCheckedIn_ThrowsInvalidOperationException()
        {
            var existing = new Attendance { AttendanceId = 1, EmployeeId = 1, Date = DateOnly.FromDateTime(DateTime.UtcNow) };
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync(existing);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CheckInAsync(1, new CheckInDto { CheckIn = new TimeSpan(9, 0, 0), WorkMode = "WFO" }));
            Assert.Equal("Already checked in today", ex.Message);
        }

        [Fact]
        public async Task CheckInAsync_WithInactiveEmployee_ThrowsInvalidOperationException()
        {
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync((Attendance?)null);
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(new Employee { EmployeeId = 1, IsActive = false });

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CheckInAsync(1, new CheckInDto { CheckIn = new TimeSpan(9, 0, 0), WorkMode = "WFO" }));
            Assert.Equal("Employee not found or inactive", ex.Message);
        }

        [Fact]
        public async Task CheckInAsync_WithValidData_CreatesAttendance()
        {
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync((Attendance?)null);
            _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(1)).ReturnsAsync(new Employee { EmployeeId = 1, IsActive = true });
            _unitOfWorkMock.Setup(u => u.Attendances.AddAsync(It.IsAny<Attendance>())).ReturnsAsync(new Attendance());
            _mapperMock.Setup(m => m.Map<AttendanceDto>(It.IsAny<Attendance>())).Returns(new AttendanceDto());

            var result = await _service.CheckInAsync(1, new CheckInDto { CheckIn = new TimeSpan(9, 0, 0), WorkMode = "WFO" });

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CheckOutAsync_WithNoCheckIn_ThrowsInvalidOperationException()
        {
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync((Attendance?)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CheckOutAsync(1, new CheckOutDto { CheckOut = new TimeSpan(17, 0, 0) }));
            Assert.Equal("No check-in record found for today", ex.Message);
        }

        [Fact]
        public async Task CheckOutAsync_WhenAlreadyCheckedOut_ThrowsInvalidOperationException()
        {
            var attendance = new Attendance { AttendanceId = 1, EmployeeId = 1, CheckIn = new TimeSpan(9, 0, 0), CheckOut = new TimeSpan(17, 0, 0) };
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync(attendance);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CheckOutAsync(1, new CheckOutDto { CheckOut = new TimeSpan(18, 0, 0) }));
            Assert.Equal("Already checked out today", ex.Message);
        }

        [Fact]
        public async Task CheckOutAsync_WithCheckOutBeforeCheckIn_ThrowsInvalidOperationException()
        {
            var attendance = new Attendance { AttendanceId = 1, EmployeeId = 1, CheckIn = new TimeSpan(9, 0, 0), CheckOut = null };
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync(attendance);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CheckOutAsync(1, new CheckOutDto { CheckOut = new TimeSpan(8, 0, 0) }));
            Assert.Equal("Check-out must be after check-in", ex.Message);
        }

        [Fact]
        public async Task CheckOutAsync_LessThan4Hours_MarksAsHalfDay()
        {
            var attendance = new Attendance { AttendanceId = 1, EmployeeId = 1, CheckIn = new TimeSpan(9, 0, 0), CheckOut = null, Status = "Present", TotalHours = 0 };
            _unitOfWorkMock.Setup(u => u.Attendances.GetTodayAttendanceAsync(1)).ReturnsAsync(attendance);
            _mapperMock.Setup(m => m.Map<AttendanceDto>(It.IsAny<Attendance>())).Returns(new AttendanceDto());

            await _service.CheckOutAsync(1, new CheckOutDto { CheckOut = new TimeSpan(12, 0, 0) });

            Assert.Equal("HalfDay", attendance.Status);
            Assert.Equal(3, attendance.TotalHours);
        }
    }
}