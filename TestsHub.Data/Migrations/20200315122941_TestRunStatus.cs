using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class TestRunStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<sbyte>(
                name: "Status",
                table: "TestRuns",
                nullable: false,
                defaultValue: (sbyte)0);
            migrationBuilder.Sql($"update  TestCases set status = 1  where Status = 'passed'");
            migrationBuilder.Sql($"update  TestCases set status = 0  where Status = 'failed'");
            migrationBuilder.Sql($"update  TestCases set status = -1  where Status = 'skipped'");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Status",
                table: "TestCases",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TestRuns");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TestCases",
                nullable: false,
                oldClrType: typeof(sbyte));
        }
    }
}
