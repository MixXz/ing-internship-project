using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class V9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Employes_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Employes_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Employes_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Employes_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Positions_PositionID",
                table: "Employes");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employes_CreatedById",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Employes_ReviewedById",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_RequstID",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employes",
                table: "Employes");

            migrationBuilder.RenameTable(
                name: "Employes",
                newName: "Employees");

            migrationBuilder.RenameColumn(
                name: "RequstID",
                table: "Responses",
                newName: "RequestID");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_RequstID",
                table: "Responses",
                newName: "IX_Responses_RequestID");

            migrationBuilder.RenameColumn(
                name: "DaysOfNumber",
                table: "Employees",
                newName: "DaysOffNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Employes_PositionID",
                table: "Employees",
                newName: "IX_Employees_PositionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employees_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Employees_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Employees_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Employees_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionID",
                table: "Employees",
                column: "PositionID",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_CreatedById",
                table: "Requests",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Employees_ReviewedById",
                table: "Responses",
                column: "ReviewedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_RequestID",
                table: "Responses",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Employees_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Employees_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Employees_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Employees_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_CreatedById",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Employees_ReviewedById",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_RequestID",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employes");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "Responses",
                newName: "RequstID");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_RequestID",
                table: "Responses",
                newName: "IX_Responses_RequstID");

            migrationBuilder.RenameColumn(
                name: "DaysOffNumber",
                table: "Employes",
                newName: "DaysOfNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PositionID",
                table: "Employes",
                newName: "IX_Employes_PositionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employes",
                table: "Employes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employes_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Employes_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Employes_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Employes_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Positions_PositionID",
                table: "Employes",
                column: "PositionID",
                principalTable: "Positions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employes_CreatedById",
                table: "Requests",
                column: "CreatedById",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Employes_ReviewedById",
                table: "Responses",
                column: "ReviewedById",
                principalTable: "Employes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_RequstID",
                table: "Responses",
                column: "RequstID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
