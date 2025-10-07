using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class document : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDocTypes_TaskTypes_TaskTypeId",
                table: "TaskDocTypes");

            migrationBuilder.DropIndex(
                name: "IX_TaskDocTypes_TaskTypeId",
                table: "TaskDocTypes");

            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                table: "TaskDocTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FormDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TaskDocTypeTaskType",
                columns: table => new
                {
                    TaskDocTypesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskTypesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDocTypeTaskType", x => new { x.TaskDocTypesId, x.TaskTypesId });
                    table.ForeignKey(
                        name: "FK_TaskDocTypeTaskType_TaskDocTypes_TaskDocTypesId",
                        column: x => x.TaskDocTypesId,
                        principalTable: "TaskDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskDocTypeTaskType_TaskTypes_TaskTypesId",
                        column: x => x.TaskTypesId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TaskDocTypeTaskType",
                columns: new[] { "TaskDocTypesId", "TaskTypesId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "1", "2" },
                    { "1", "3" },
                    { "2", "2" },
                    { "2", "3" },
                    { "3", "2" },
                    { "4", "2" },
                    { "4", "4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocTypeTaskType_TaskTypesId",
                table: "TaskDocTypeTaskType",
                column: "TaskTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDocTypeTaskType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FormDocuments");

            migrationBuilder.AddColumn<string>(
                name: "TaskTypeId",
                table: "TaskDocTypes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TaskDocTypes",
                keyColumn: "Id",
                keyValue: "1",
                column: "TaskTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskDocTypes",
                keyColumn: "Id",
                keyValue: "2",
                column: "TaskTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskDocTypes",
                keyColumn: "Id",
                keyValue: "3",
                column: "TaskTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskDocTypes",
                keyColumn: "Id",
                keyValue: "4",
                column: "TaskTypeId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocTypes_TaskTypeId",
                table: "TaskDocTypes",
                column: "TaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDocTypes_TaskTypes_TaskTypeId",
                table: "TaskDocTypes",
                column: "TaskTypeId",
                principalTable: "TaskTypes",
                principalColumn: "Id");
        }
    }
}
