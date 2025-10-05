using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class tasktype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskTypes");

            migrationBuilder.AddColumn<string>(
                name: "ValidationErrorMessage",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "1",
                column: "ValidationErrorMessage",
                value: "Name should only contain letters!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "2",
                column: "ValidationErrorMessage",
                value: "Name should only contain letters!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "3",
                column: "ValidationErrorMessage",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "4",
                column: "ValidationErrorMessage",
                value: "Doesn't match phone number pattern!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "5",
                column: "ValidationErrorMessage",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "6",
                column: "ValidationErrorMessage",
                value: "Field should only contain letters!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "7",
                column: "ValidationErrorMessage",
                value: "Field should only contain letters!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "8",
                column: "ValidationErrorMessage",
                value: "Field should only contain numbers!");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "9",
                column: "ValidationErrorMessage",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "1",
                column: "StarterTeamId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "2",
                column: "StarterTeamId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "3",
                column: "StarterTeamId",
                value: "2");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "4",
                column: "StarterTeamId",
                value: "3");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "5",
                column: "StarterTeamId",
                value: "4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationErrorMessage",
                table: "TaskFields");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Description", "StarterTeamId" },
                values: new object[] { "Plan, create, and schedule posts for social media platforms.", null });

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Description", "StarterTeamId" },
                values: new object[] { "Create and manage an advertising campaign across different channels.", null });

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "Description", "StarterTeamId" },
                values: new object[] { "Design and send promotional emails to targeted audiences.", null });

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "Description", "StarterTeamId" },
                values: new object[] { "Improve website SEO through keyword research and content updates.", null });

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "5",
                columns: new[] { "Description", "StarterTeamId" },
                values: new object[] { "Analyze competitors, customer behavior, and industry trends.", null });
        }
    }
}
