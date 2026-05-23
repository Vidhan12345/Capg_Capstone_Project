using WMS.Application.DTOs;

namespace WMS.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceDto> CheckInAsync(int employeeId, CheckInDto dto);
        Task<AttendanceDto> CheckOutAsync(int employeeId, CheckOutDto dto);
        Task<AttendanceDto?> GetTodayAttendanceAsync(int employeeId);
        Task<IEnumerable<AttendanceDto>> GetByEmployeeAsync(int employeeId, DateOnly? from, DateOnly? to);
    }
}
