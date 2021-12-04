using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class Facilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Contact_HasValue = table.Column<bool>(nullable: true),
                    Contact_Name_HasValue = table.Column<bool>(nullable: true),
                    Contact_Name_Prefix = table.Column<string>(maxLength: 64, nullable: true),
                    Contact_Name_First = table.Column<string>(maxLength: 64, nullable: true),
                    Contact_Name_Middle = table.Column<string>(maxLength: 64, nullable: true),
                    Contact_Name_Last = table.Column<string>(maxLength: 64, nullable: true),
                    Contact_Email_HasValue = table.Column<bool>(nullable: true),
                    Contact_Email_Value = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facilities");
        }
    }
}
