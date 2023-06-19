using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class FKChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_StudentAccountId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_StudentAccountId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "StudentAccountId",
                table: "Results");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Results",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Results_StudentId",
                table: "Results",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_StudentId",
                table: "Results",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_StudentId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_StudentId",
                table: "Results");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Results",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "StudentAccountId",
                table: "Results",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_StudentAccountId",
                table: "Results",
                column: "StudentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_StudentAccountId",
                table: "Results",
                column: "StudentAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
