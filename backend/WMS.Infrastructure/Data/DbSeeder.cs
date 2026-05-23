using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(WMSDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Roles.AnyAsync())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin", Description = "System Administrator" },
                    new Role { Name = "Manager", Description = "Department Manager" },
                    new Role { Name = "Employee", Description = "Regular Employee" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Departments.AnyAsync())
            {
                context.Departments.Add(
                    new Department { Name = "General", Description = "General Department" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Employees.AnyAsync())
            {
                var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
                var generalDept = await context.Departments.FirstAsync();

                var admin = new Employee
                {
                    EmployeeCode = "ADM001",
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@wms.com",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "M",
                    DateOfJoining = new DateTime(2024, 1, 1),
                    IsActive = true,
                    DepartmentId = generalDept.DepartmentId,
                    RoleId = adminRole.RoleId
                };

                context.Employees.Add(admin);
                await context.SaveChangesAsync();

                if (!await context.UserLogins.AnyAsync())
                {
                    context.UserLogins.Add(new UserLogin
                    {
                        EmployeeId = admin.EmployeeId,
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        RoleId = adminRole.RoleId
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
