using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Email_HasValue = table.Column<bool>(nullable: true),
                    Email_Value = table.Column<string>(maxLength: 256, nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    OnCallProviderId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
