namespace WMS.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        IAttendanceRepository Attendances { get; }
        ILeaveRepository Leaves { get; }
        IProjectRepository Projects { get; }
        IDepartmentRepository Departments { get; }
        IRoleRepository Roles { get; }
        IClientRepository Clients { get; }
        IAnnouncementRepository Announcements { get; }
        IUserLoginRepository UserLogins { get; }
        IAuditLogRepository AuditLogs { get; }
        Task<int> CompleteAsync();
    }
}
