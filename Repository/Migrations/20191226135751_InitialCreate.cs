using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookService.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Isbn = table.Column<string>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "round(extract(epoch from now()))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
