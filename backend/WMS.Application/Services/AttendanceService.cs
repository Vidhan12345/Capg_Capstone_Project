using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceDto> CheckInAsync(int employeeId, CheckInDto dto)
        {
            var existing = await _unitOfWork.Attendances.GetTodayAttendanceAsync(employeeId);
            if (existing != null)
                throw new InvalidOperationException("Already checked in today");

            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null || !employee.IsActive)
                throw new InvalidOperationException("Employee not found or inactive");

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                CheckIn = dto.CheckIn,
                WorkMode = dto.WorkMode,
                Status = "Present",
                TotalHours = 0
            };

            await _unitOfWork.Attendances.AddAsync(attendance);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AttendanceDto>(attendance);
        }

        public async Task<AttendanceDto> CheckOutAsync(int employeeId, CheckOutDto dto)
        {
            var attendance = await _unitOfWork.Attendances.GetTodayAttendanceAsync(employeeId);
            if (attendance == null)
                throw new InvalidOperationException("No check-in record found for today");

            if (attendance.CheckOut != null)
                throw new InvalidOperationException("Already checked out today");

            if (dto.CheckOut <= attendance.CheckIn)
                throw new InvalidOperationException("Check-out must be after check-in");

            attendance.CheckOut = dto.CheckOut;
            attendance.TotalHours = (decimal)(dto.CheckOut - attendance.CheckIn).TotalHours;
            if (attendance.TotalHours < 4)
                attendance.Status = "HalfDay";

            await _unitOfWork.Attendances.UpdateAsync(attendance);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AttendanceDto>(attendance);
        }

        public async Task<AttendanceDto?> GetTodayAttendanceAsync(int employeeId)
        {
            var attendance = await _unitOfWork.Attendances.GetTodayAttendanceAsync(employeeId);
            return attendance == null ? null : _mapper.Map<AttendanceDto>(attendance);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByEmployeeAsync(int employeeId, DateOnly? from, DateOnly? to)
        {
            var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
            var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var records = await _unitOfWork.Attendances.GetByEmployeeAsync(employeeId, fromDate, toDate);
            return _mapper.Map<IEnumerable<AttendanceDto>>(records);
        }
    }
}
