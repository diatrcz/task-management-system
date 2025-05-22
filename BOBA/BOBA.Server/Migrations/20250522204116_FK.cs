using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "FormFields",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModelId",
                table: "FormFields",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "FormDocuments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DocTypeId",
                table: "FormDocuments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_ModelId",
                table: "FormFields",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_TaskId",
                table: "FormFields",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDocuments_DocTypeId",
                table: "FormDocuments",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDocuments_TaskId",
                table: "FormDocuments",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormDocuments_TaskDocTypes_DocTypeId",
                table: "FormDocuments",
                column: "DocTypeId",
                principalTable: "TaskDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormDocuments_Tasks_TaskId",
                table: "FormDocuments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_TaskFields_ModelId",
                table: "FormFields",
                column: "ModelId",
                principalTable: "TaskFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_Tasks_TaskId",
                table: "FormFields",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormDocuments_TaskDocTypes_DocTypeId",
                table: "FormDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_FormDocuments_Tasks_TaskId",
                table: "FormDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_TaskFields_ModelId",
                table: "FormFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_Tasks_TaskId",
                table: "FormFields");

            migrationBuilder.DropIndex(
                name: "IX_FormFields_ModelId",
                table: "FormFields");

            migrationBuilder.DropIndex(
                name: "IX_FormFields_TaskId",
                table: "FormFields");

            migrationBuilder.DropIndex(
                name: "IX_FormDocuments_DocTypeId",
                table: "FormDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FormDocuments_TaskId",
                table: "FormDocuments");

            migrationBuilder.DropColumn(
                name: "DocTypeId",
                table: "FormDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ModelId",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "FormDocuments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
