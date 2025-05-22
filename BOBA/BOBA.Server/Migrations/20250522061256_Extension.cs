using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class Extension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskflowTeam_TaskFlows_ReadOnlyRoleWorkflowsId",
                table: "TaskflowTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskflowTeam_Teams_ReadOnlyRoleId",
                table: "TaskflowTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskflowTeam",
                table: "TaskflowTeam");

            migrationBuilder.RenameTable(
                name: "TaskflowTeam",
                newName: "TaskFlowTeam");

            migrationBuilder.RenameIndex(
                name: "IX_TaskflowTeam_ReadOnlyRoleWorkflowsId",
                table: "TaskFlowTeam",
                newName: "IX_TaskFlowTeam_ReadOnlyRoleWorkflowsId");

            migrationBuilder.AddColumn<string>(
                name: "FormFieldJson",
                table: "TaskFlows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Validation",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "TaskFields",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskFlowTeam",
                table: "TaskFlowTeam",
                columns: new[] { "ReadOnlyRoleId", "ReadOnlyRoleWorkflowsId" });

            migrationBuilder.CreateTable(
                name: "FormDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UploadeddAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploaderId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormFields",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Validation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskDocTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDocTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskDocTypes_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "TaskDocTypes",
                columns: new[] { "Id", "Description", "Name", "TaskTypeId", "Type" },
                values: new object[,]
                {
                    { "1", "A PDF document outlining the campaign overview and goals.", "Campaign Brief", null, "pdf" },
                    { "2", "A Word document containing the legal agreement with the client.", "Client Contract", null, "docx" },
                    { "3", "A PowerPoint presentation of design drafts and visual ideas.", "Design Mockup", null, "pptx" },
                    { "4", "An Excel spreadsheet with performance analytics and KPIs.", "Performance Report", null, "xlsx" }
                });

            migrationBuilder.InsertData(
                table: "TaskFields",
                columns: new[] { "Id", "Description", "Name", "Options", "Validation" },
                values: new object[,]
                {
                    { "1", "Deadline for task completion.", "Due Date", null, "^\\d{4}-\\d{2}-\\d{2}$" },
                    { "2", "Task urgency level.", "Priority", "Low,Medium,High,Urgent", "^(Low|Medium|High|Urgent)$" },
                    { "3", "Person responsible for the task.", "Assigned To", null, null },
                    { "4", "Marketing channel the task belongs to.", "Channel", "Email,Social Media,SEO,Paid Ads,Events", "^(Email|Social Media|SEO|Paid Ads|Events)$" },
                    { "5", "Estimated task budget.", "Budget", null, "^\\d+(\\.\\d{1,2})?$" }
                });

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "1",
                column: "FormFieldJson",
                value: "[{\"Id\":\"personalInfo\",\"Layout\":{\"Type\":\"grid\",\"Columns\":2,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"firstName\",\"Type\":\"text\",\"Label\":\"First Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"lastName\",\"Type\":\"text\",\"Label\":\"Last Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null},{\"Id\":\"contactInfo\",\"Layout\":{\"Type\":\"full-width\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"email\",\"Type\":\"email\",\"Label\":\"Email Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"phone\",\"Type\":\"tel\",\"Label\":\"Phone Number\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null},{\"Id\":\"addressInfo\",\"Layout\":{\"Type\":\"combined\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"address\",\"Type\":\"text\",\"Label\":\"Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":[{\"Id\":\"cityStateZip\",\"Layout\":{\"Type\":\"grid\",\"Columns\":3,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"city\",\"Type\":\"text\",\"Label\":\"City\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"state\",\"Type\":\"text\",\"Label\":\"State\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"zip\",\"Type\":\"text\",\"Label\":\"Zip Code\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null}]},{\"Id\":\"additionalInfo\",\"Layout\":{\"Type\":\"full-width\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"notes\",\"Type\":\"textarea\",\"Label\":\"Additional Notes\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":4,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "2",
                column: "FormFieldJson",
                value: "[{\"Id\":\"personalInfo\",\"Layout\":{\"Type\":\"grid\",\"Columns\":2,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"firstName\",\"Type\":\"text\",\"Label\":\"First Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"lastName\",\"Type\":\"text\",\"Label\":\"Last Name\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null},{\"Id\":\"contactInfo\",\"Layout\":{\"Type\":\"full-width\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"email\",\"Type\":\"email\",\"Label\":\"Email Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"phone\",\"Type\":\"tel\",\"Label\":\"Phone Number\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null},{\"Id\":\"addressInfo\",\"Layout\":{\"Type\":\"combined\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"address\",\"Type\":\"text\",\"Label\":\"Address\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":[{\"Id\":\"cityStateZip\",\"Layout\":{\"Type\":\"grid\",\"Columns\":3,\"GapClasses\":\"gap-6\"},\"Fields\":[{\"Id\":\"city\",\"Type\":\"text\",\"Label\":\"City\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"state\",\"Type\":\"text\",\"Label\":\"State\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}},{\"Id\":\"zip\",\"Type\":\"text\",\"Label\":\"Zip Code\",\"Placeholder\":\"\",\"Required\":true,\"Disabled\":false,\"Rows\":null,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null}]},{\"Id\":\"additionalInfo\",\"Layout\":{\"Type\":\"full-width\",\"Columns\":null,\"GapClasses\":null},\"Fields\":[{\"Id\":\"notes\",\"Type\":\"textarea\",\"Label\":\"Additional Notes\",\"Placeholder\":\"\",\"Required\":false,\"Disabled\":false,\"Rows\":4,\"StyleClasses\":{\"Container\":\"\",\"Label\":\"block text-sm font-medium text-gray-700 mb-1\",\"Input\":\"w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-pink-200 focus:border-pink-300\"}}],\"SubSections\":null}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "3",
                column: "FormFieldJson",
                value: "null");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "4",
                column: "FormFieldJson",
                value: "null");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "5",
                column: "FormFieldJson",
                value: "null");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "6",
                column: "FormFieldJson",
                value: "null");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocTypes_TaskTypeId",
                table: "TaskDocTypes",
                column: "TaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFlowTeam_TaskFlows_ReadOnlyRoleWorkflowsId",
                table: "TaskFlowTeam",
                column: "ReadOnlyRoleWorkflowsId",
                principalTable: "TaskFlows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskFlowTeam_Teams_ReadOnlyRoleId",
                table: "TaskFlowTeam",
                column: "ReadOnlyRoleId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskFlowTeam_TaskFlows_ReadOnlyRoleWorkflowsId",
                table: "TaskFlowTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskFlowTeam_Teams_ReadOnlyRoleId",
                table: "TaskFlowTeam");

            migrationBuilder.DropTable(
                name: "FormDocuments");

            migrationBuilder.DropTable(
                name: "FormFields");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "TaskDocTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskFlowTeam",
                table: "TaskFlowTeam");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "TaskFields",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DropColumn(
                name: "FormFieldJson",
                table: "TaskFlows");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "TaskFields");

            migrationBuilder.RenameTable(
                name: "TaskFlowTeam",
                newName: "TaskflowTeam");

            migrationBuilder.RenameIndex(
                name: "IX_TaskFlowTeam_ReadOnlyRoleWorkflowsId",
                table: "TaskflowTeam",
                newName: "IX_TaskflowTeam_ReadOnlyRoleWorkflowsId");

            migrationBuilder.AlterColumn<string>(
                name: "Validation",
                table: "TaskFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskflowTeam",
                table: "TaskflowTeam",
                columns: new[] { "ReadOnlyRoleId", "ReadOnlyRoleWorkflowsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskflowTeam_TaskFlows_ReadOnlyRoleWorkflowsId",
                table: "TaskflowTeam",
                column: "ReadOnlyRoleWorkflowsId",
                principalTable: "TaskFlows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskflowTeam_Teams_ReadOnlyRoleId",
                table: "TaskflowTeam",
                column: "ReadOnlyRoleId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
