using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFirstDataAccess.Migrations
{
    public partial class AddingResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Assessment1 = table.Column<double>(type: "double", nullable: false),
                    Assessment2 = table.Column<double>(type: "double", nullable: false),
                    Test = table.Column<double>(type: "double", nullable: false),
                    Examination = table.Column<double>(type: "double", nullable: false),
                    Total = table.Column<double>(type: "double", nullable: false),
                    Position = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Grade = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionYearId = table.Column<int>(type: "int", nullable: false),
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    SubClassId = table.Column<int>(type: "int", nullable: false),
                    SubClassesId = table.Column<int>(type: "int", nullable: false),
                    IsAddedBefore = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TermRegId = table.Column<int>(type: "int", nullable: false),
                    ExamsOfficer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultTables_ClassesInSchools_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "ClassesInSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultTables_SessionYears_SessionYearId",
                        column: x => x.SessionYearId,
                        principalTable: "SessionYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultTables_SubClasses_SubClassesId",
                        column: x => x.SubClassesId,
                        principalTable: "SubClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultTables_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultTables_Termregistrations_TermRegId",
                        column: x => x.TermRegId,
                        principalTable: "Termregistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_ClassesId",
                table: "ResultTables",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_IsAddedBefore",
                table: "ResultTables",
                column: "IsAddedBefore",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_SessionYearId",
                table: "ResultTables",
                column: "SessionYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_SubClassesId",
                table: "ResultTables",
                column: "SubClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_SubjectId",
                table: "ResultTables",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_TermRegId",
                table: "ResultTables",
                column: "TermRegId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultTables");
        }
    }
}
