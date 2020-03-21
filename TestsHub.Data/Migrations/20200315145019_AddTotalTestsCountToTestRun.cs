using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHub.Data.Migrations
{
    public partial class AddTotalTestsCountToTestRun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TestCasesCount",
                table: "TestRuns",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coverage_TestRunId",
                table: "Coverage");

            migrationBuilder.DropColumn(
                name: "TestCasesCount",
                table: "TestRuns");

            migrationBuilder.CreateIndex(
                name: "IX_Coverage_TestRunId",
                table: "Coverage",
                column: "TestRunId");
        }
    }
}
