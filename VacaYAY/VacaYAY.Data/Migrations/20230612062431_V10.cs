using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a9761be1-bf87-4536-afa7-bd714ac37b2d", "6f73b364-41fc-4a4e-8af2-25dd5285a9ed", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[] { 1, "HR Manager", "Managing HR operations and employee relations." });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "47880f16-74e9-4dd2-a185-20149f2e372f", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 12, 8, 24, 31, 37, DateTimeKind.Local).AddTicks(8155), "Root", "999999", new DateTime(2023, 6, 12, 8, 24, 31, 37, DateTimeKind.Local).AddTicks(8207), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAEGg+rAiJF/F3N8wZOC9CavsmpmduFX+0GosFKeleatzJKDwSucd6yFdig9NbKujXNA==", null, false, 1, "0c007a51-6e30-4db0-a320-e953ef95b66d", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a9761be1-bf87-4536-afa7-bd714ac37b2d", "47880f16-74e9-4dd2-a185-20149f2e372f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a9761be1-bf87-4536-afa7-bd714ac37b2d", "47880f16-74e9-4dd2-a185-20149f2e372f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9761be1-bf87-4536-afa7-bd714ac37b2d");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "47880f16-74e9-4dd2-a185-20149f2e372f");

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
