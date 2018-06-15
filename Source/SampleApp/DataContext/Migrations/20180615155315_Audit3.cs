using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class Audit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aggregate",
                table: "AuditEntries",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AggregateId",
                table: "AuditEntries",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aggregate",
                table: "AuditEntries");

            migrationBuilder.DropColumn(
                name: "AggregateId",
                table: "AuditEntries");
        }
    }
}
