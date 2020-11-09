using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class CustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LtcPharmacies_Customers_DbCustomerId",
                table: "LtcPharmacies");

            migrationBuilder.DropIndex(
                name: "IX_LtcPharmacies_DbCustomerId",
                table: "LtcPharmacies");

            migrationBuilder.DropColumn(
                name: "DbCustomerId",
                table: "LtcPharmacies");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "LtcPharmacies",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AuditRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhenUtc = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 64, nullable: true),
                    Aggregate = table.Column<string>(maxLength: 64, nullable: true),
                    AggregateId = table.Column<string>(maxLength: 64, nullable: true),
                    Entity = table.Column<string>(maxLength: 64, nullable: true),
                    EntityId = table.Column<string>(maxLength: 64, nullable: true),
                    Event = table.Column<string>(maxLength: 64, nullable: true),
                    Changes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LtcPharmacies_CustomerId",
                table: "LtcPharmacies",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LtcPharmacies_Customers_CustomerId",
                table: "LtcPharmacies",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LtcPharmacies_Customers_CustomerId",
                table: "LtcPharmacies");

            migrationBuilder.DropTable(
                name: "AuditRecords");

            migrationBuilder.DropIndex(
                name: "IX_LtcPharmacies_CustomerId",
                table: "LtcPharmacies");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "LtcPharmacies");

            migrationBuilder.AddColumn<Guid>(
                name: "DbCustomerId",
                table: "LtcPharmacies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LtcPharmacies_DbCustomerId",
                table: "LtcPharmacies",
                column: "DbCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LtcPharmacies_Customers_DbCustomerId",
                table: "LtcPharmacies",
                column: "DbCustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
