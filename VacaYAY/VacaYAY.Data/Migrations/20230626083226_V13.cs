using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c4a9a388-0fdd-4a27-bc99-9cfa4c1f594e", "f8afb56c-8a05-49b7-a494-8b14812f8244" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4a9a388-0fdd-4a27-bc99-9cfa4c1f594e");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "f8afb56c-8a05-49b7-a494-8b14812f8244");

            migrationBuilder.AddColumn<int>(
                name: "NumOfDaysRemovedFromNewDaysOff",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfDaysRemovedFromOldDaysOff",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldDaysOffNumber",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "43355af8-d0c1-488e-b031-81ca7bb60fa0", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OldDaysOffNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ace61721-7b40-4bf1-bead-af8cb24468fe", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 26, 10, 32, 26, 480, DateTimeKind.Local).AddTicks(295), "Root", "999999", new DateTime(2023, 6, 26, 10, 32, 26, 480, DateTimeKind.Local).AddTicks(355), "Root", false, null, null, "ROOT@ROOT.COM", 0, "AQAAAAIAAYagAAAAEBUQqIFA0KloOQJ9y50KFIP/3yfFT9b8up35nZk609TvPP7b4bu7GoJjM492sWVG2A==", null, false, 1, "42bbdbc0-69f5-48f9-a2c7-8a2732fadc95", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "ace61721-7b40-4bf1-bead-af8cb24468fe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "ace61721-7b40-4bf1-bead-af8cb24468fe" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6b6aaa4-a62d-4d7c-b183-c359946abb3b");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ace61721-7b40-4bf1-bead-af8cb24468fe");

            migrationBuilder.DropColumn(
                name: "NumOfDaysRemovedFromNewDaysOff",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "NumOfDaysRemovedFromOldDaysOff",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "OldDaysOffNumber",
                table: "Employees");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c4a9a388-0fdd-4a27-bc99-9cfa4c1f594e", "5007fdc1-6c8a-4585-8a1b-1a4638a3ba39", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f8afb56c-8a05-49b7-a494-8b14812f8244", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 20, 13, 57, 42, 456, DateTimeKind.Local).AddTicks(632), "Root", "999999", new DateTime(2023, 6, 20, 13, 57, 42, 456, DateTimeKind.Local).AddTicks(696), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAEGawDavfC1M2HxKg6x7n2C/b4jnHFRfsYGlXgVkBQMt0QxCJzY/ThI3JulxFDYlSUg==", null, false, 1, "01a59995-d983-4cc4-83f0-74fa1a8863b2", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c4a9a388-0fdd-4a27-bc99-9cfa4c1f594e", "f8afb56c-8a05-49b7-a494-8b14812f8244" });
        }
    }
}
