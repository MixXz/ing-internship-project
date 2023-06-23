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
                name: "OldDaysOffNumber",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2fcc7a3b-69ef-4055-992b-d4223f8664b2", "6bd28a78-dbef-4aa8-840f-66f04c6f9fb2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OldDaysOffNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4013e13a-dab2-4ed2-8254-34b486cd79e2", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 23, 13, 58, 16, 59, DateTimeKind.Local).AddTicks(5196), "Root", "999999", new DateTime(2023, 6, 23, 13, 58, 16, 59, DateTimeKind.Local).AddTicks(5259), "Root", false, null, null, "ROOT@ROOT.COM", 0, "AQAAAAIAAYagAAAAEHwpIZptZrGG18bzndcBG/mFDfppJS53+zZmYVxNiERdqYkrK1cs0NmHZry95jpw4g==", null, false, 1, "d9d96939-95e4-4ef9-938e-dbaede3b45b5", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2fcc7a3b-69ef-4055-992b-d4223f8664b2", "4013e13a-dab2-4ed2-8254-34b486cd79e2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2fcc7a3b-69ef-4055-992b-d4223f8664b2", "4013e13a-dab2-4ed2-8254-34b486cd79e2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fcc7a3b-69ef-4055-992b-d4223f8664b2");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "4013e13a-dab2-4ed2-8254-34b486cd79e2");

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
