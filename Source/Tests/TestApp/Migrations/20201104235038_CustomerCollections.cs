using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class CustomerCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SomeInts",
                table: "Customers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LtcPharmacies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    DbCustomerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtcPharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LtcPharmacies_Customers_DbCustomerId",
                        column: x => x.DbCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LtcPharmacies_DbCustomerId",
                table: "LtcPharmacies",
                column: "DbCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LtcPharmacies");

            migrationBuilder.DropColumn(
                name: "SomeInts",
                table: "Customers");
        }
    }
}
