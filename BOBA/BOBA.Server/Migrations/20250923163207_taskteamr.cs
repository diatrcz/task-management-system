using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class taskteamr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorTeamId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorTeamId",
                table: "Tasks");
        }
    }
}
