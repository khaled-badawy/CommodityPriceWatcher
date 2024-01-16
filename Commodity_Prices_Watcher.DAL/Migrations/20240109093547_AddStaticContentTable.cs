using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commodity_Prices_Watcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                table: "CommodityCategory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "StaticContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouterLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortIndex = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticContent", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaticContent");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                table: "CommodityCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
