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
                values: new object[] { "138a5f64-0d8b-4de0-a9c2-aee3ee19644f", "67021ab6-bf64-4b20-b22b-eb29edab63b3", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[] { 1, "HR", "HR" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bb51f6a2-5cc3-4211-bf29-179217797c0d", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 10, 14, 5, 34, 457, DateTimeKind.Local).AddTicks(6457), "Root", "000", new DateTime(2023, 6, 10, 14, 5, 34, 457, DateTimeKind.Local).AddTicks(6519), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAECSlxD0UvubhwNwzIwI5IkIecVevXYmmb9tIxoNiz2EEfrIlFV/s9OQh93qf7741jw==", null, false, 1, "cd9fd541-c0fc-4519-8d7d-760f3b997f94", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "138a5f64-0d8b-4de0-a9c2-aee3ee19644f", "bb51f6a2-5cc3-4211-bf29-179217797c0d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "138a5f64-0d8b-4de0-a9c2-aee3ee19644f", "bb51f6a2-5cc3-4211-bf29-179217797c0d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "138a5f64-0d8b-4de0-a9c2-aee3ee19644f");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "bb51f6a2-5cc3-4211-bf29-179217797c0d");

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
