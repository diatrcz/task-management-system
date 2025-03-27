using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Choice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Choices",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { "1", "Approve the task and move it forward to the next phase of the workflow.", "Approve, Proceed to Next Phase" },
                    { "2", "The task requires additional review before proceeding further.", "Needs Further Review" },
                    { "3", "The task does not meet the requirements and needs to be completely reworked.", "Reject, Task Requires Redoing" },
                    { "4", "Approve the task but with some minor revisions or improvements.", "Approve with Minor Adjustments" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Choices",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Choices",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Choices",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Choices",
                keyColumn: "Id",
                keyValue: "4");
        }
    }
}
