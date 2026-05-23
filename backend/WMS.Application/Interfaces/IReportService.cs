using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;

namespace WMS.Application.Interfaces
{
    public interface IReportService
    {
        Task<PagedResult<AttendanceReportDto>> GetAttendanceReportAsync(AttendanceReportFilterDto filter);
        Task<PagedResult<LeaveReportDto>> GetLeaveReportAsync(LeaveReportFilterDto filter);
        Task<PagedResult<EmployeeReportDto>> GetEmployeeReportAsync(EmployeeReportFilterDto filter);
    }
}