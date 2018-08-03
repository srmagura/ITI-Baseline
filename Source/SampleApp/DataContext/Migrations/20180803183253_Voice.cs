using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class Voice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoiceRecords",
                columns: table => new
                {
                    DateCreatedUtc = table.Column<DateTimeOffset>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(nullable: false),
                    SentUtc = table.Column<DateTimeOffset>(nullable: true),
                    ToAddress = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    RetryCount = table.Column<int>(nullable: false),
                    NextRetry = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceRecords");
        }
    }
}
