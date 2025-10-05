using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class fieldseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskFields");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Label", "Name", "Placeholder", "Type", "Validation" },
                values: new object[] { "First Name", "firstName", null, "text", "^[A-Za-z]+$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Label", "Name", "Options", "Placeholder", "Type", "Validation" },
                values: new object[] { "Last Name", "lastName", null, null, "text", "^[A-Za-z]+$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "Label", "Name", "Placeholder", "Type", "Validation" },
                values: new object[] { "E-mail", "email", "email@email.com", "email", "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "Label", "Name", "Options", "Placeholder", "Type", "Validation" },
                values: new object[] { "Phone Number", "phone", null, "+36201234567", "text", "^\\+?[0-9\\s\\-()]{7,15}$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "5",
                columns: new[] { "Label", "Name", "Placeholder", "Type", "Validation" },
                values: new object[] { "Address", "address", null, "text", null });

            migrationBuilder.InsertData(
                table: "TaskFields",
                columns: new[] { "Id", "Label", "Name", "Options", "Placeholder", "Type", "Validation" },
                values: new object[,]
                {
                    { "6", "City", "city", null, null, "text", "^[A-Za-z]+$" },
                    { "7", "State", "state", null, null, "text", "^[A-Za-z]+$" },
                    { "8", "Zip", "zip", null, null, "text", "^[0-9]+$" },
                    { "9", "Additional Notes", "notes", null, null, "textarea", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "TaskFields");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "TaskFields");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "TaskFields");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Description", "Name", "Validation" },
                values: new object[] { "Deadline for task completion.", "Due Date", "^\\d{4}-\\d{2}-\\d{2}$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "Description", "Name", "Options", "Validation" },
                values: new object[] { "Task urgency level.", "Priority", "Low,Medium,High,Urgent", "^(Low|Medium|High|Urgent)$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "Description", "Name", "Validation" },
                values: new object[] { "Person responsible for the task.", "Assigned To", null });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "Description", "Name", "Options", "Validation" },
                values: new object[] { "Marketing channel the task belongs to.", "Channel", "Email,Social Media,SEO,Paid Ads,Events", "^(Email|Social Media|SEO|Paid Ads|Events)$" });

            migrationBuilder.UpdateData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "5",
                columns: new[] { "Description", "Name", "Validation" },
                values: new object[] { "Estimated task budget.", "Budget", "^\\d+(\\.\\d{1,2})?$" });
        }
    }
}
