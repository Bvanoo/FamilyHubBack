using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamHubBack.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecretSantaAddIsReveal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevealed",
                table: "SecretSantaDraws",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevealed",
                table: "SecretSantaDraws");
        }
    }
}
