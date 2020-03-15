using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class AddBranchAndCommitId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "TestRuns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitId",
                table: "TestRuns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "TestRuns");

            migrationBuilder.DropColumn(
                name: "CommitId",
                table: "TestRuns");
        }
    }
}
