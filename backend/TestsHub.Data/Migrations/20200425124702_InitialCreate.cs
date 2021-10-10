using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHub.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                    table.UniqueConstraint("UX_Org_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TestSuite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Hostname = table.Column<string>(nullable: true),
                    Package = table.Column<string>(nullable: true),
                    JUnitId = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Time = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSuite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false, maxLength: 256),
                    OrganisationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    
                    table.UniqueConstraint("UX_Project_Unique_Index", x => new { x.OrganisationId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "TestRuns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TestRunName = table.Column<string>(nullable: false, maxLength: 256),
                    ProjectId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Time = table.Column<decimal>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    TestCasesCount = table.Column<int>(nullable: false),
                    Branch = table.Column<string>(nullable: true),
                    CommitId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestRuns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.UniqueConstraint("UX_TestRun_Unique_Index", x => new { x.ProjectId , x.TestRunName });
                });

            migrationBuilder.CreateTable(
                name: "Coverage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TestRunId = table.Column<int>(nullable: false),
                    LinesCovered = table.Column<int>(nullable: false),
                    LinesValid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coverage_TestRuns_TestRunId",
                        column: x => x.TestRunId,
                        principalTable: "TestRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TestRunId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    SystemOut = table.Column<string>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Time = table.Column<decimal>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    TestSuiteId = table.Column<int>(nullable: true),
                    TestOutput = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_TestRuns_TestRunId",
                        column: x => x.TestRunId,
                        principalTable: "TestRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestCases_TestSuite_TestSuiteId",
                        column: x => x.TestSuiteId,
                        principalTable: "TestSuite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);                    
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coverage_TestRunId",
                table: "Coverage",
                column: "TestRunId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganisationId",
                table: "Projects",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TestRunId",
                table: "TestCases",
                column: "TestRunId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TestSuiteId",
                table: "TestCases",
                column: "TestSuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_ProjectId",
                table: "TestRuns",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coverage");

            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "TestRuns");

            migrationBuilder.DropTable(
                name: "TestSuite");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Organisations");
        }
    }
}
