using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "5ddb61dd-5267-46b0-817a-1e55523ed08c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "765f1edd-f273-4d1c-9c02-273efa61040f");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "5ddb61dd-5267-46b0-817a-1e55523ed08c");

            migrationBuilder.AddColumn<int>(
                name: "NotificationStatus",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "761b7813-8415-4885-bab1-81c504c2309e", "4c601686-4b42-4c0a-bced-cb1b066134cd", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b4f4387a-b198-41de-b63e-0fe3d3c93708", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 15, 9, 33, 22, 928, DateTimeKind.Local).AddTicks(431), "Root", "999999", new DateTime(2023, 6, 15, 9, 33, 22, 928, DateTimeKind.Local).AddTicks(487), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAECIOgaG9BEhKUo7gHnv1oQuMHSNSs7KTNDf8Fq5kDRLdXeHqNhqqW7LQBGvcFMezcQ==", null, false, 1, "36b7d3b8-7841-4360-a15b-22c5cfdbd1c3", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "761b7813-8415-4885-bab1-81c504c2309e", "b4f4387a-b198-41de-b63e-0fe3d3c93708" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "761b7813-8415-4885-bab1-81c504c2309e", "b4f4387a-b198-41de-b63e-0fe3d3c93708" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "761b7813-8415-4885-bab1-81c504c2309e");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "b4f4387a-b198-41de-b63e-0fe3d3c93708");

            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "Requests");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "e1f51e25-648b-4232-b6e0-3bdab5cdccdc", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5ddb61dd-5267-46b0-817a-1e55523ed08c", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 12, 15, 56, 55, 612, DateTimeKind.Local).AddTicks(4502), "Root", "999999", new DateTime(2023, 6, 12, 15, 56, 55, 612, DateTimeKind.Local).AddTicks(4559), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAEOZ5Z8D5ilxF+etfqyeoJ33bb1w5QY9VW5uk1Qror2durcTSubWB7CFzaUQlOxiypA==", null, false, 1, "3d14748a-9e5f-4f86-a649-a27e828edd99", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "765f1edd-f273-4d1c-9c02-273efa61040f", "5ddb61dd-5267-46b0-817a-1e55523ed08c" });
        }
    }
}
