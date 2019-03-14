using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class FooValPar2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValueParent_Child_ChildValue",
                table: "Foos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueParent_ParentValue",
                table: "Foos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueParent_Child_ChildValue",
                table: "Foos");

            migrationBuilder.DropColumn(
                name: "ValueParent_ParentValue",
                table: "Foos");
        }
    }
}
