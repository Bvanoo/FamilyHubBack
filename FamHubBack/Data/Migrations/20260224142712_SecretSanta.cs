using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamHubBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecretSanta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecretSantaDraws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    GiverId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    DrawDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretSantaDraws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecretSantaDraws_AspNetUsers_GiverId",
                        column: x => x.GiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretSantaDraws_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretSantaDraws_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaDraws_GiverId",
                table: "SecretSantaDraws",
                column: "GiverId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaDraws_GroupId",
                table: "SecretSantaDraws",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaDraws_ReceiverId",
                table: "SecretSantaDraws",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecretSantaDraws");
        }
    }
}
