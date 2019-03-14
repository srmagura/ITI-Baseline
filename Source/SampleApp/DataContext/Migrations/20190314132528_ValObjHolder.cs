using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class ValObjHolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValObjHolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address_Line1 = table.Column<string>(maxLength: 64, nullable: true),
                    Address_Line2 = table.Column<string>(maxLength: 64, nullable: true),
                    Address_City = table.Column<string>(maxLength: 64, nullable: true),
                    Address_State = table.Column<string>(maxLength: 16, nullable: true),
                    Address_Zip = table.Column<string>(maxLength: 16, nullable: true),
                    PersonName_Prefix = table.Column<string>(maxLength: 64, nullable: true),
                    PersonName_First = table.Column<string>(maxLength: 64, nullable: true),
                    PersonName_Middle = table.Column<string>(maxLength: 64, nullable: true),
                    PersonName_Last = table.Column<string>(maxLength: 64, nullable: true),
                    PhoneNumber_Value = table.Column<string>(maxLength: 16, nullable: true),
                    ValueParent_ParentValue = table.Column<string>(nullable: true),
                    ValueParent_Child_ChildValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValObjHolders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValObjHolders");
        }
    }
}
