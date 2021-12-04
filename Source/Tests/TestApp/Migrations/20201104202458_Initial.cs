using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    NotInEntity = table.Column<string>(maxLength: 64, nullable: true),
                    SimplePersonName_HasValue = table.Column<bool>(nullable: true),
                    SimplePersonName_Prefix = table.Column<string>(maxLength: 64, nullable: true),
                    SimplePersonName_First = table.Column<string>(maxLength: 64, nullable: true),
                    SimplePersonName_Middle = table.Column<string>(maxLength: 64, nullable: true),
                    SimplePersonName_Last = table.Column<string>(maxLength: 64, nullable: true),
                    PhoneNumber_HasValue = table.Column<bool>(nullable: true),
                    PhoneNumber_Value = table.Column<string>(maxLength: 16, nullable: true),
                    SimpleAddress_HasValue = table.Column<bool>(nullable: true),
                    SimpleAddress_Line1 = table.Column<string>(maxLength: 64, nullable: true),
                    SimpleAddress_Line2 = table.Column<string>(maxLength: 64, nullable: true),
                    SimpleAddress_City = table.Column<string>(maxLength: 64, nullable: true),
                    SimpleAddress_State = table.Column<string>(maxLength: 16, nullable: true),
                    SimpleAddress_Zip = table.Column<string>(maxLength: 16, nullable: true),
                    SomeMoney = table.Column<decimal>(type: "Money", nullable: false),
                    SomeNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
