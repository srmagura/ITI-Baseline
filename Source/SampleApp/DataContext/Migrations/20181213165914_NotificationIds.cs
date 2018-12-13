using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class NotificationIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                table: "VoiceRecords",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                table: "SmsRecords",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                table: "EmailRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "VoiceRecords");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "SmsRecords");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "EmailRecords");
        }
    }
}
