using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class RemoveValObjId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Id",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "PersonName_Id",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "PhoneNumber_Id",
                table: "Foos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Address_Id",
                table: "Foos",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PersonName_Id",
                table: "Foos",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhoneNumber_Id",
                table: "Foos",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
