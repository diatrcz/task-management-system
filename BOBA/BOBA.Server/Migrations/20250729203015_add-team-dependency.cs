using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class addteamdependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Tasks");
        }
    }
}
