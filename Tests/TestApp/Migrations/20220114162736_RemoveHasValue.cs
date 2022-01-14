using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApp.Migrations
{
    public partial class RemoveHasValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_HasValue",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Thread",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Contact_Email_HasValue",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Contact_HasValue",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Contact_Name_HasValue",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Address_HasValue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactName_HasValue",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactPhone_HasValue",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Email_HasValue",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thread",
                table: "LogEntries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Contact_Email_HasValue",
                table: "Facilities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Contact_HasValue",
                table: "Facilities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Contact_Name_HasValue",
                table: "Facilities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Address_HasValue",
                table: "Customers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ContactName_HasValue",
                table: "Customers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ContactPhone_HasValue",
                table: "Customers",
                type: "bit",
                nullable: true);
        }
    }
}
