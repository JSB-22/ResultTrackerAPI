using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class Fixedissueontopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_Subjects_SubjectId",
                table: "Result");

            migrationBuilder.DropForeignKey(
                name: "FK_Result_Topic_TopicID",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Result",
                table: "Result");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.RenameTable(
                name: "Result",
                newName: "Results");

            migrationBuilder.RenameIndex(
                name: "IX_Result_TopicID",
                table: "Results",
                newName: "IX_Results_TopicID");

            migrationBuilder.RenameIndex(
                name: "IX_Result_SubjectId",
                table: "Results",
                newName: "IX_Results_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Subjects_SubjectId",
                table: "Results",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Topics_TopicID",
                table: "Results",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Subjects_SubjectId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Topics_TopicID",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "Result");

            migrationBuilder.RenameIndex(
                name: "IX_Results_TopicID",
                table: "Result",
                newName: "IX_Result_TopicID");

            migrationBuilder.RenameIndex(
                name: "IX_Results_SubjectId",
                table: "Result",
                newName: "IX_Result_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Result",
                table: "Result",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Result_Subjects_SubjectId",
                table: "Result",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Result_Topic_TopicID",
                table: "Result",
                column: "TopicID",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
