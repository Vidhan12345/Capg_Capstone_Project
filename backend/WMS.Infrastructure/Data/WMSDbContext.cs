using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public class WMSDbContext : DbContext
    {
        public WMSDbContext(DbContextOptions<WMSDbContext> options) : base(options) { }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<Leave> Leaves => Set<Leave>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<EmployeeProjectAllocation> EmployeeProjectAllocations => Set<EmployeeProjectAllocation>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
        public DbSet<UserLogin> UserLogins => Set<UserLogin>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasQueryFilter(e => !e.IsDeleted);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.Gender).HasMaxLength(1);
                entity.Property(e => e.DateOfJoining).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.EmergencyContact).HasMaxLength(100);
                entity.Property(e => e.ProfileImage).HasMaxLength(500);
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasIndex(e => e.EmployeeCode).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Employees)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendanceId);
                entity.Property(e => e.TotalHours).HasColumnType("decimal(5,2)");
                entity.Property(e => e.WorkMode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.Attendances)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.EmployeeId, e.Date }).IsUnique();
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(e => e.LeaveId);
                entity.Property(e => e.LeaveType).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.Leaves)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Approver)
                    .WithMany(emp => emp.ApprovedLeaves)
                    .HasForeignKey(e => e.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);
                entity.Property(e => e.ClientName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.ClientAddress).HasMaxLength(500);
                entity.Property(e => e.ClientPhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ClientLocation).HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired();
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmployeeProjectAllocation>(entity =>
            {
                entity.HasKey(e => e.AllocationId);

                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.Allocations)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.EmployeeId, e.ProjectId }).IsUnique();
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.AnnouncementId);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Content).HasMaxLength(5000).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Ignore(e => e.IsDeleted);

                entity.HasOne(e => e.PostedByEmployee)
                    .WithMany(emp => emp.Announcements)
                    .HasForeignKey(e => e.PostedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.HasOne(e => e.Employee)
                    .WithOne(emp => emp.UserLogin)
                    .HasForeignKey<UserLogin>(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserLogins)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.EmployeeId).IsUnique();
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditId);
                entity.Property(e => e.EntityName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.RecordId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Action).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired(false);

                entity.HasOne(e => e.CreatedByEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.EntityName, e.RecordId });
                entity.HasIndex(e => e.CreatedOn);
            });
        }
    }
}
