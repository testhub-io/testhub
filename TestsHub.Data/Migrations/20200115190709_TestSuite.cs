using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHub.Data.Migrations
{
    public partial class TestSuite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hostname",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "JUnitId",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "TestRuns");

            migrationBuilder.AddColumn<int>(
                name: "TestSuiteId",
                table: "TestCases",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestSuite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
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

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TestSuiteId",
                table: "TestCases",
                column: "TestSuiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCases_TestSuite_TestSuiteId",
                table: "TestCases",
                column: "TestSuiteId",
                principalTable: "TestSuite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCases_TestSuite_TestSuiteId",
                table: "TestCases");

            migrationBuilder.DropTable(
                name: "TestSuite");

            migrationBuilder.DropIndex(
                name: "IX_TestCases_TestSuiteId",
                table: "TestCases");

            migrationBuilder.DropColumn(
                name: "TestSuiteId",
                table: "TestCases");

            migrationBuilder.AddColumn<string>(
                name: "Hostname",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JUnitId",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "TestRuns",
                nullable: true);
        }
    }
}
