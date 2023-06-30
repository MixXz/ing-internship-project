using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85c9d842-a904-49cc-b412-3ad0b3012fc8", "a1511504-005b-4666-bb10-69eb0c5e9f16", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OldDaysOffNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f3518c1e-1c6a-4479-8557-625c673e1720", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 29, 14, 23, 41, 945, DateTimeKind.Local).AddTicks(631), "Root", "999999", new DateTime(2023, 6, 29, 14, 23, 41, 945, DateTimeKind.Local).AddTicks(717), "Root", false, null, null, "ROOT@ROOT.COM", 0, "AQAAAAIAAYagAAAAEGRYO43Z5Lavn5pxFdUXe1JXRTq2knKbRNu1KSDtDJH9pXQSZtQAiqepehXoEzDJLg==", null, false, 1, "f0422ff2-a945-4bba-948b-2e725b08f3d2", false, "root@root.com" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Collective Vacation", "Opisa bez" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 2,
                column: "Caption",
                value: "Sick Leave");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 3,
                column: "Caption",
                value: "Days off");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 4,
                column: "Caption",
                value: "Paid leave");

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[] { 5, "Unpaid leave", "Neki opis" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "85c9d842-a904-49cc-b412-3ad0b3012fc8", "f3518c1e-1c6a-4479-8557-625c673e1720" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "85c9d842-a904-49cc-b412-3ad0b3012fc8", "f3518c1e-1c6a-4479-8557-625c673e1720" });

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85c9d842-a904-49cc-b412-3ad0b3012fc8");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "f3518c1e-1c6a-4479-8557-625c673e1720");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "43355af8-d0c1-488e-b031-81ca7bb60fa0", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OldDaysOffNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ace61721-7b40-4bf1-bead-af8cb24468fe", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 26, 10, 32, 26, 480, DateTimeKind.Local).AddTicks(295), "Root", "999999", new DateTime(2023, 6, 26, 10, 32, 26, 480, DateTimeKind.Local).AddTicks(355), "Root", false, null, null, "ROOT@ROOT.COM", 0, "AQAAAAIAAYagAAAAEBUQqIFA0KloOQJ9y50KFIP/3yfFT9b8up35nZk609TvPP7b4bu7GoJjM492sWVG2A==", null, false, 1, "42bbdbc0-69f5-48f9-a2c7-8a2732fadc95", false, "root@root.com" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Sick Leave", "Neki opis" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 2,
                column: "Caption",
                value: "Days off");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 3,
                column: "Caption",
                value: "Paid leave");

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 4,
                column: "Caption",
                value: "Unpaid leave");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "ace61721-7b40-4bf1-bead-af8cb24468fe" });
        }
    }
}
