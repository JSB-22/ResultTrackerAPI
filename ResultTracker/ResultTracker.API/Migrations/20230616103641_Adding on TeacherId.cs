using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResultTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddingonTeacherId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2cd8a78-af3b-4088-9f65-323f3d7c49b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3ac5e91-8ccb-4eca-8a2b-6dbb5d155b3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7dc84a1-aa48-4576-b482-e424dc697675");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeacherId",
                table: "AspNetUsers",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_TeacherId",
                table: "AspNetUsers",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_TeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c2cd8a78-af3b-4088-9f65-323f3d7c49b9", "c2cd8a78-af3b-4088-9f65-323f3d7c49b9", "Student", "STUDENT" },
                    { "d3ac5e91-8ccb-4eca-8a2b-6dbb5d155b3b", "d3ac5e91-8ccb-4eca-8a2b-6dbb5d155b3b", "Teacher", "TEACHER" },
                    { "d7dc84a1-aa48-4576-b482-e424dc697675", "d7dc84a1-aa48-4576-b482-e424dc697675", "Admin", "ADMIN" }
                });
        }
    }
}
