using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.DTOs.Common;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<AttendanceReportDto>> GetAttendanceReportAsync(AttendanceReportFilterDto filter)
        {
            var fromDate = filter.FromDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));
            var toDate = filter.ToDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var allRecords = await _unitOfWork.Attendances.GetByDateRangeAsync(fromDate, toDate);

            var query = allRecords.AsEnumerable();

            if (filter.EmployeeId.HasValue)
                query = query.Where(a => a.EmployeeId == filter.EmployeeId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(a => a.Employee.DepartmentId == filter.DepartmentId.Value);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(a => a.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.WorkMode))
                query = query.Where(a => a.WorkMode == filter.WorkMode);

            var list = query.ToList();
            var total = list.Count;
            var items = list.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToList();

            return new PagedResult<AttendanceReportDto>
            {
                Items = _mapper.Map<IEnumerable<AttendanceReportDto>>(items),
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<PagedResult<LeaveReportDto>> GetLeaveReportAsync(LeaveReportFilterDto filter)
        {
            var fromDate = filter.FromDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6));
            var toDate = filter.ToDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var leaves = await _unitOfWork.Leaves.FindAsync(l =>
                l.FromDate >= fromDate && l.ToDate <= toDate);

            var query = leaves.AsEnumerable();

            if (filter.EmployeeId.HasValue)
                query = query.Where(l => l.EmployeeId == filter.EmployeeId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(l => l.Employee.DepartmentId == filter.DepartmentId.Value);

            if (!string.IsNullOrEmpty(filter.LeaveType))
                query = query.Where(l => l.LeaveType == filter.LeaveType);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(l => l.Status == filter.Status);

            var list = query.ToList();
            var total = list.Count;
            var items = list.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToList();

            return new PagedResult<LeaveReportDto>
            {
                Items = _mapper.Map<IEnumerable<LeaveReportDto>>(items),
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<PagedResult<EmployeeReportDto>> GetEmployeeReportAsync(EmployeeReportFilterDto filter)
        {
            var (employees, total) = await _unitOfWork.Employees.GetPagedAsync(
                filter.Page, filter.PageSize,
                filter: e =>
                    (!filter.DepartmentId.HasValue || e.DepartmentId == filter.DepartmentId.Value) &&
                    (!filter.RoleId.HasValue || e.RoleId == filter.RoleId.Value) &&
                    (!filter.IsActive.HasValue || e.IsActive == filter.IsActive.Value) &&
                    (string.IsNullOrEmpty(filter.Search) ||
                     (e.FirstName + " " + e.LastName).Contains(filter.Search) ||
                     e.Email.Contains(filter.Search) ||
                     e.EmployeeCode.Contains(filter.Search)));

            var items = _mapper.Map<IEnumerable<EmployeeReportDto>>(employees);
            foreach (var dto in items)
            {
                dto.TotalProjects = (await _unitOfWork.Projects.FindAsync(
                    p => p.Allocations.Any(a => a.EmployeeId == dto.EmployeeId))).Count();
                dto.TotalLeaves = await _unitOfWork.Leaves.CountAsync(l => l.EmployeeId == dto.EmployeeId);
                dto.TotalAttendance = await _unitOfWork.Attendances.CountAsync(a => a.EmployeeId == dto.EmployeeId);
            }

            return new PagedResult<EmployeeReportDto>
            {
                Items = items,
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}