using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderDOJIsActiveRoleIdAndFixTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename Phone → PhoneNumber (preserves existing data)
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Employees",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserLogins",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfJoining",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            // Convert existing Client Status values ('Active'→1, 'Inactive'→0) before altering type
            migrationBuilder.Sql("UPDATE Clients SET Status = 1 WHERE Status = 'Active'");
            migrationBuilder.Sql("UPDATE Clients SET Status = 0 WHERE Status = 'Inactive' OR Status IS NULL OR Status = ''");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Clients",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            // Clear AuditLog CreatedBy string values before altering to int
            migrationBuilder.Sql("UPDATE AuditLogs SET CreatedBy = NULL WHERE ISNUMERIC(CreatedBy) = 0 OR CreatedBy IS NULL");
            migrationBuilder.Sql("UPDATE AuditLogs SET CreatedBy = CAST(CreatedBy AS INT) WHERE ISNUMERIC(CreatedBy) = 1");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "AuditLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_RoleId",
                table: "UserLogins",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Employees_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Roles_RoleId",
                table: "UserLogins",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Employees_CreatedBy",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Roles_RoleId",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_RoleId",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "DateOfJoining",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Employees",
                newName: "Phone");

            migrationBuilder.AlterColumn<string>(
                name: "RoleOnProject",
                table: "EmployeeProjectAllocations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "AuditLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
