using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsDoIt.Moody.Persistance.Migrations
{
    public partial class AddDbSetEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailVerificaitonTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(maxLength: 1000, nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificaitonTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerificaitonTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificaitonTokens_UserId",
                table: "EmailVerificaitonTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerificaitonTokens");
        }
    }
}
