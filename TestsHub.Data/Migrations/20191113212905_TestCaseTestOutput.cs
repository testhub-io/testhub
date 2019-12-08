using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class TestCaseTestOutput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestOutput",
                table: "TestCases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestOutput",
                table: "TestCases");
        }
    }
}
