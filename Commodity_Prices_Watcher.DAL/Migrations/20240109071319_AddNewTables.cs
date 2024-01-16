using Microsoft.EntityFrameworkCore.Migrations;
using System.Spatial;

#nullable disable

namespace Commodity_Prices_Watcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterDatabase(
            //    oldCollation: "Arabic_CI_AS");

            migrationBuilder.CreateTable(
                name: "CommodityCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommodityCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommodityCategoryId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Shape = table.Column<Geometry>(type: "geography", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPrice_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SharedPrice_CommodityCategory_CommodityCategoryId",
                        column: x => x.CommodityCategoryId,
                        principalTable: "CommodityCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentsLookUp",
                columns: table => new
                {
                    SharedPriceId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentsLookUp", x => new { x.SharedPriceId, x.Id });
                    table.ForeignKey(
                        name: "FK_AttachmentsLookUp_SharedPrice_SharedPriceId",
                        column: x => x.SharedPriceId,
                        principalTable: "SharedPrice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedPrice_ApplicationUserId",
                table: "SharedPrice",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPrice_CommodityCategoryId",
                table: "SharedPrice",
                column: "CommodityCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentsLookUp");

            migrationBuilder.DropTable(
                name: "SharedPrice");

            migrationBuilder.DropTable(
                name: "CommodityCategory");

            //migrationBuilder.AlterDatabase(
            //    collation: "Arabic_CI_AS");
        }
    }
}
