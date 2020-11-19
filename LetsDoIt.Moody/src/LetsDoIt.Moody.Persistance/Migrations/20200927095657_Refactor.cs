using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsDoIt.Moody.Persistance.Migrations
{
    public partial class Refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetail_Categories_CategoryId",
                table: "CategoryDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetail",
                table: "CategoryDetail");

            migrationBuilder.RenameTable(
                name: "CategoryDetail",
                newName: "CategoryDetails");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetail_CategoryId",
                table: "CategoryDetails",
                newName: "IX_CategoryDetails_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetails_Categories_CategoryId",
                table: "CategoryDetails",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetails_Categories_CategoryId",
                table: "CategoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails");

            migrationBuilder.RenameTable(
                name: "CategoryDetails",
                newName: "CategoryDetail");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetails_CategoryId",
                table: "CategoryDetail",
                newName: "IX_CategoryDetail_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetail",
                table: "CategoryDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetail_Categories_CategoryId",
                table: "CategoryDetail",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
