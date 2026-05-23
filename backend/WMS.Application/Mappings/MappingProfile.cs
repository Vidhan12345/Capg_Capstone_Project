using AutoMapper;
using WMS.Application.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name))
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name));
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.EmployeeCount, o => o.MapFrom(s => s.Employees.Count(e => !e.IsDeleted)));
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();

            CreateMap<Attendance, AttendanceDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName));

            CreateMap<Leave, LeaveDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ForMember(d => d.ApprovedByName, o => o.MapFrom(s => s.Approver != null ? s.Approver.FirstName + " " + s.Approver.LastName : null));
            CreateMap<ApplyLeaveDto, Leave>();

            CreateMap<Project, ProjectDto>()
                .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.ClientName));
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            CreateMap<Client, ClientDto>();
            CreateMap<CreateClientDto, Client>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status ?? true));
            CreateMap<UpdateClientDto, Client>();

            CreateMap<EmployeeProjectAllocation, AllocationDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name));

            CreateMap<Announcement, AnnouncementDto>()
                .ForMember(d => d.PostedByName, o => o.MapFrom(s => s.PostedByEmployee.FirstName + " " + s.PostedByEmployee.LastName));
            CreateMap<UpdateAnnouncementDto, Announcement>();

            CreateMap<Role, RoleDto>();

            CreateMap<DashboardStats, DashboardStatsDto>();
            CreateMap<DashboardTrend, AttendanceTrendDto>();
            CreateMap<DashboardDepartmentDistribution, DepartmentDistributionDto>();

            CreateMap<AuditLog, AuditLogDto>()
                .ForMember(d => d.CreatedByName, o => o.MapFrom(s => s.CreatedByEmployee != null ? s.CreatedByEmployee.FirstName + " " + s.CreatedByEmployee.LastName : null));

            CreateMap<Attendance, AttendanceReportDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.Employee.EmployeeCode))
                .ForMember(d => d.Department, o => o.MapFrom(s => s.Employee.Department.Name));
            CreateMap<Leave, LeaveReportDto>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.FirstName + " " + s.Employee.LastName))
                .ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.Employee.EmployeeCode))
                .ForMember(d => d.Department, o => o.MapFrom(s => s.Employee.Department.Name))
                .ForMember(d => d.ApprovedByName, o => o.MapFrom(s => s.Approver != null ? s.Approver.FirstName + " " + s.Approver.LastName : null))
                .ForMember(d => d.DaysRequested, o => o.MapFrom(s => (s.ToDate.DayNumber - s.FromDate.DayNumber) + 1));
            CreateMap<Employee, EmployeeReportDto>()
                .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department.Name))
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
                .ForMember(d => d.TotalProjects, o => o.Ignore())
                .ForMember(d => d.TotalLeaves, o => o.Ignore())
                .ForMember(d => d.TotalAttendance, o => o.Ignore());
        }
    }
}
