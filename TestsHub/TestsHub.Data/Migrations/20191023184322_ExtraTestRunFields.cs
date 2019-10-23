using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class ExtraTestRunFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hostname",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JUnitId",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "TestRuns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hostname",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "JUnitId",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "TestRuns");
        }
    }
}
