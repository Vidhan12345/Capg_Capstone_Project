using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WMSDbContext _context;

        public IEmployeeRepository Employees { get; }
        public IAttendanceRepository Attendances { get; }
        public ILeaveRepository Leaves { get; }
        public IProjectRepository Projects { get; }
        public IDepartmentRepository Departments { get; }
        public IRoleRepository Roles { get; }
        public IClientRepository Clients { get; }
        public IAnnouncementRepository Announcements { get; }
        public IUserLoginRepository UserLogins { get; }
        public IAuditLogRepository AuditLogs { get; }

        public UnitOfWork(
            WMSDbContext context,
            IEmployeeRepository employees,
            IAttendanceRepository attendances,
            ILeaveRepository leaves,
            IProjectRepository projects,
            IDepartmentRepository departments,
            IRoleRepository roles,
            IClientRepository clients,
            IAnnouncementRepository announcements,
            IUserLoginRepository userLogins,
            IAuditLogRepository auditLogs)
        {
            _context = context;
            Employees = employees;
            Attendances = attendances;
            Leaves = leaves;
            Projects = projects;
            Departments = departments;
            Roles = roles;
            Clients = clients;
            Announcements = announcements;
            UserLogins = userLogins;
            AuditLogs = auditLogs;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
