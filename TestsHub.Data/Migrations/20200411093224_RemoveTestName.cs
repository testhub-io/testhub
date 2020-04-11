using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHub.Data.Migrations
{
    public partial class RemoveTestName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {         
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TestRuns");         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TestRuns",
                nullable: true);
        }
    }
}
