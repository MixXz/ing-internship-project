using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveTypes_LeaveTypes_LeaveTypeID",
                table: "LeaveTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_LeaveTypes_LeaveTypeID",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_LeaveTypeID",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_LeaveTypeID",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "LeaveTypeID",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "LeaveTypeID",
                table: "LeaveTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeID",
                table: "Responses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeID",
                table: "LeaveTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_LeaveTypeID",
                table: "Responses",
                column: "LeaveTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_LeaveTypeID",
                table: "LeaveTypes",
                column: "LeaveTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveTypes_LeaveTypes_LeaveTypeID",
                table: "LeaveTypes",
                column: "LeaveTypeID",
                principalTable: "LeaveTypes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_LeaveTypes_LeaveTypeID",
                table: "Responses",
                column: "LeaveTypeID",
                principalTable: "LeaveTypes",
                principalColumn: "ID");
        }
    }
}
