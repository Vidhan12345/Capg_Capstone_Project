using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAuditLogAndClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Clients",
                newName: "ClientPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "ClientName");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Clients",
                newName: "ClientAddress");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "AuditLogs",
                newName: "RecordId");

            migrationBuilder.RenameColumn(
                name: "ChangedBy",
                table: "AuditLogs",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "ChangedAt",
                table: "AuditLogs",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "ClientLocation",
                table: "Clients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedOn",
                table: "AuditLogs",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_RecordId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "RecordId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CreatedOn",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityName_RecordId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "ClientLocation",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "ClientPhoneNumber",
                table: "Clients",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ClientAddress",
                table: "Clients",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "AuditLogs",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "AuditLogs",
                newName: "ChangedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "AuditLogs",
                newName: "ChangedBy");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
