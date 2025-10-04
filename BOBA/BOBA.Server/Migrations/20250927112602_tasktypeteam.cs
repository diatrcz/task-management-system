using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOBA.Server.Migrations
{
    /// <inheritdoc />
    public partial class tasktypeteam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validation",
                table: "FormFields");

            migrationBuilder.AddColumn<string>(
                name: "StarterTeamId",
                table: "TaskTypes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifierId",
                table: "FormFields",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "1",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"2\",\"EditRoleId\":\"2\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "2",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"3\",\"EditRoleId\":\"3\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"1\",\"EditRoleId\":\"1\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\",\"EditRoleId\":null}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "3",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"4\",\"EditRoleId\":\"1\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"2\",\"EditRoleId\":\"2\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "4",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"5\",\"EditRoleId\":\"2\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"3\",\"EditRoleId\":\"3\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "5",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"6\",\"EditRoleId\":\"3\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"4\",\"EditRoleId\":\"1\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\",\"EditRoleId\":null}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "6",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"8\",\"EditRoleId\":null},{\"ChoiceId\":\"2\",\"NextStateId\":\"5\",\"EditRoleId\":\"2\"}]");

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "1",
                column: "StarterTeamId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "2",
                column: "StarterTeamId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "3",
                column: "StarterTeamId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "4",
                column: "StarterTeamId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskTypes",
                keyColumn: "Id",
                keyValue: "5",
                column: "StarterTeamId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypes_StarterTeamId",
                table: "TaskTypes",
                column: "StarterTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_ModifierId",
                table: "FormFields",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_AspNetUsers_ModifierId",
                table: "FormFields",
                column: "ModifierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTypes_Teams_StarterTeamId",
                table: "TaskTypes",
                column: "StarterTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_AspNetUsers_ModifierId",
                table: "FormFields");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTypes_Teams_StarterTeamId",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_TaskTypes_StarterTeamId",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_FormFields_ModifierId",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "StarterTeamId",
                table: "TaskTypes");

            migrationBuilder.AlterColumn<string>(
                name: "ModifierId",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Validation",
                table: "FormFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "1",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"2\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "2",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"3\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"1\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "3",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"4\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"2\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "4",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"5\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"3\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "5",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"6\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"4\"},{\"ChoiceId\":\"3\",\"NextStateId\":\"9\"}]");

            migrationBuilder.UpdateData(
                table: "TaskFlows",
                keyColumn: "Id",
                keyValue: "6",
                column: "NextStateJson",
                value: "[{\"ChoiceId\":\"1\",\"NextStateId\":\"8\"},{\"ChoiceId\":\"2\",\"NextStateId\":\"5\"}]");
        }
    }
}
