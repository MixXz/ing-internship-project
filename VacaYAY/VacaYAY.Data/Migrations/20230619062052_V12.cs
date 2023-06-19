using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractType = table.Column<int>(type: "int", nullable: false),
                    DocumentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contracts_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88c3dd0a-80eb-4112-806b-1757b0902cd7", "abcdf04d-41d2-40f3-b1e5-04ece9c0f3c6", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmployeeEndDate", "EmployeeStartDate", "FirstName", "IDNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8205c01e-d38f-4e4f-adb8-283817d7d4c0", 0, "Root", null, 22, null, "root@root.com", true, null, new DateTime(2023, 6, 19, 8, 20, 52, 397, DateTimeKind.Local).AddTicks(4292), "Root", "999999", new DateTime(2023, 6, 19, 8, 20, 52, 397, DateTimeKind.Local).AddTicks(4360), "Root", false, null, null, "ROOT@ROOT.COM", "AQAAAAIAAYagAAAAEMlARDChGfVm25654m04q1ohU4wEJLST6/DR0MvtLwJWgE7oqz6t4eWUFVT/r9F7qg==", null, false, 1, "e0af7edd-fed8-4cae-8a0a-9423b18c8e0f", false, "root@root.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "88c3dd0a-80eb-4112-806b-1757b0902cd7", "8205c01e-d38f-4e4f-adb8-283817d7d4c0" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EmployeeID",
                table: "Contracts",
                column: "EmployeeID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "88c3dd0a-80eb-4112-806b-1757b0902cd7", "8205c01e-d38f-4e4f-adb8-283817d7d4c0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88c3dd0a-80eb-4112-806b-1757b0902cd7");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "8205c01e-d38f-4e4f-adb8-283817d7d4c0");

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
    }
}
