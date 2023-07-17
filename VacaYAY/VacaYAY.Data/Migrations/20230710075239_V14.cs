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
                values: new object[] { "16f58530-02ce-44dc-a08f-b06b69986279", "8008e641-c97c-4975-9ca7-8b11d66458ab", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OldDaysOffNumber", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a700eb88-5fa4-49d4-9890-08a011bc9dbb", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 7, 10, 9, 52, 39, 644, DateTimeKind.Local).AddTicks(1865), "Root", "999999", new DateTime(2023, 7, 10, 9, 52, 39, 644, DateTimeKind.Local).AddTicks(1923), "Root", false, null, null, "ROOT@ROOT.COM", 0, "AQAAAAIAAYagAAAAED+Sl8tap/IKx7COhI9lLFfzMqEFsJzlcLynTb7IxMOfbd3L5E21V4BQ9RYdigeX/w==", null, false, 1, "b45b27e0-d329-4186-b8da-99924a9122ab", false, "root@root.com" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Days off", "Some desc." });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Paid leave", "Some desc." });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Unpaid leave", "Some desc." });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Sick Leave", "Some desc." });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "ID", "Caption", "Description" },
                values: new object[] { 5, "Collective Vacation", "Collective Vacation" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "16f58530-02ce-44dc-a08f-b06b69986279", "a700eb88-5fa4-49d4-9890-08a011bc9dbb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "16f58530-02ce-44dc-a08f-b06b69986279", "a700eb88-5fa4-49d4-9890-08a011bc9dbb" });

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16f58530-02ce-44dc-a08f-b06b69986279");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "a700eb88-5fa4-49d4-9890-08a011bc9dbb");

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
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Days off", "Neki opis" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Paid leave", "Neki opis" });

            migrationBuilder.UpdateData(
                table: "LeaveTypes",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "Caption", "Description" },
                values: new object[] { "Unpaid leave", "Neki opis" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a6b6aaa4-a62d-4d7c-b183-c359946abb3b", "ace61721-7b40-4bf1-bead-af8cb24468fe" });
        }
    }
}
