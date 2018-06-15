using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class Catchup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "AuditEntries",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuditEntries",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditEntries");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AuditEntries",
                newName: "User");
        }
    }
}
