using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class SmsRetryUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextRetry",
                table: "VoiceRecords",
                newName: "NextRetryUtc");

            migrationBuilder.RenameColumn(
                name: "NextRetry",
                table: "SmsRecords",
                newName: "NextRetryUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextRetryUtc",
                table: "VoiceRecords",
                newName: "NextRetry");

            migrationBuilder.RenameColumn(
                name: "NextRetryUtc",
                table: "SmsRecords",
                newName: "NextRetry");
        }
    }
}
