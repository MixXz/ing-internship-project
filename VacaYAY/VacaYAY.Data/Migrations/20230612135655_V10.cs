using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                values: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "e1f51e25-648b-4232-b6e0-3bdab5cdccdc", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[,]
                {
                    { 1, "Sick Leave", "Neki opis" },
                    { 2, "Days off", "Neki opis" },
                    { 3, "Paid leave", "Neki opis" },
                    { 4, "Unpaid leave", "Neki opis" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[,]
                {
                    { 1, "HR Manager", "Managing HR operations and employee relations." },
                    { 2, "Software Engineer", "Responsible for developing software applications." },
                    { 3, "Project Manager", "Leading project teams and ensuring project success." }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5ddb61dd-5267-46b0-817a-1e55523ed08c", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 12, 15, 56, 55, 612, DateTimeKind.Local).AddTicks(4502), "Root", "999999", new DateTime(2023, 6, 12, 15, 56, 55, 612, DateTimeKind.Local).AddTicks(4559), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAEOZ5Z8D5ilxF+etfqyeoJ33bb1w5QY9VW5uk1Qror2durcTSubWB7CFzaUQlOxiypA==", null, false, 1, "3d14748a-9e5f-4f86-a649-a27e828edd99", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "5ddb61dd-5267-46b0-817a-1e55523ed08c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "5ddb61dd-5267-46b0-817a-1e55523ed08c" });

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "765f1edd-f273-4d1c-9c02-273efa61040f");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "5ddb61dd-5267-46b0-817a-1e55523ed08c");

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
