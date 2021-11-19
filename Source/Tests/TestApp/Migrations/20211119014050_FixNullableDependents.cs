using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApp.Migrations
{
    public partial class FixNullableDependents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_Name_Prefix",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ContactName_Prefix",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_Aggregate_AggregateId",
                table: "AuditRecords",
                columns: new[] { "Aggregate", "AggregateId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_Entity_EntityId",
                table: "AuditRecords",
                columns: new[] { "Entity", "EntityId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditRecords_Aggregate_AggregateId",
                table: "AuditRecords");

            migrationBuilder.DropIndex(
                name: "IX_AuditRecords_Entity_EntityId",
                table: "AuditRecords");

            migrationBuilder.AddColumn<string>(
                name: "Contact_Name_Prefix",
                table: "Facilities",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName_Prefix",
                table: "Customers",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);
        }
    }
}
