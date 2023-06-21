using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedOnAccountAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Results",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Results",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AccountId",
                table: "Results",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_AspNetUsers_AccountId",
                table: "Results",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_AspNetUsers_AccountId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_AccountId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Results");
        }
    }
}
