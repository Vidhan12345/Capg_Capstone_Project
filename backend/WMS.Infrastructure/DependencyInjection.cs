using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Repositories;
using WMS.Infrastructure.Services;

namespace WMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<WMSDbContext>((sp, options) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });

    options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
});

            services.AddScoped<AuditInterceptor>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
