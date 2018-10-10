using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class emailRetryUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextRetry",
                table: "EmailRecords",
                newName: "NextRetryUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextRetryUtc",
                table: "EmailRecords",
                newName: "NextRetry");
        }
    }
}
