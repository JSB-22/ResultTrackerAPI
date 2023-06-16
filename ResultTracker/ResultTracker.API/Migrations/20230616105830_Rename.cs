using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_AccountId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Results",
                newName: "StudentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_AccountId",
                table: "Results",
                newName: "IX_Results_StudentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_StudentAccountId",
                table: "Results",
                column: "StudentAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_StudentAccountId",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "StudentAccountId",
                table: "Results",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Results_StudentAccountId",
                table: "Results",
                newName: "IX_Results_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_AccountId",
                table: "Results",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
