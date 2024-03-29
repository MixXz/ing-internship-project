﻿using System;
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
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contracts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EmployeeId",
                table: "Contracts",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

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
