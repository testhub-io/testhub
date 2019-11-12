using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class TestCaseResultNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TestCases",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TestCases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TestCases",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TestCases",
                nullable: false);
        }
    }
}
