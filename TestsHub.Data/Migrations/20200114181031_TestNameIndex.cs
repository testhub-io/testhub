using Microsoft.EntityFrameworkCore.Migrations;

namespace TestsHub.Data.Migrations
{
    public partial class TestNameIndex : Migration
    {
        private const string IndexName = "TestCases_Name_ix";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(IndexName,               
            // table: "TestCases",
            // column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(IndexName);
        }
    }
}
