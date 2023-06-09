using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFirstDataAccess.Migrations
{
    public partial class AddingResultTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultTables_SubClasses_SubClassesId",
                table: "ResultTables");

            migrationBuilder.DropIndex(
                name: "IX_ResultTables_SubClassesId",
                table: "ResultTables");

            migrationBuilder.DropColumn(
                name: "SubClassesId",
                table: "ResultTables");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_SubClassId",
                table: "ResultTables",
                column: "SubClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultTables_SubClasses_SubClassId",
                table: "ResultTables",
                column: "SubClassId",
                principalTable: "SubClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResultTables_SubClasses_SubClassId",
                table: "ResultTables");

            migrationBuilder.DropIndex(
                name: "IX_ResultTables_SubClassId",
                table: "ResultTables");

            migrationBuilder.AddColumn<int>(
                name: "SubClassesId",
                table: "ResultTables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_SubClassesId",
                table: "ResultTables",
                column: "SubClassesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultTables_SubClasses_SubClassesId",
                table: "ResultTables",
                column: "SubClassesId",
                principalTable: "SubClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
