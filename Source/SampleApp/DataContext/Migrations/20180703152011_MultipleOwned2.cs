using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class MultipleOwned2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonName_First",
                table: "Foos",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonName_Last",
                table: "Foos",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonName_Middle",
                table: "Foos",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonName_Prefix",
                table: "Foos",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonName_First",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "PersonName_Last",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "PersonName_Middle",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "PersonName_Prefix",
                table: "Foos");
        }
    }
}
