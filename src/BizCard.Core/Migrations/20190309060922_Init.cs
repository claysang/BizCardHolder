using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BizCard.Core.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");
        }
    }
}
