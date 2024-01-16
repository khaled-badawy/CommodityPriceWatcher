using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Commodity_Prices_Watcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeShapeAttributeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttachmentsLookUp",
                table: "AttachmentsLookUp");

            migrationBuilder.AlterColumn<string>(
                name: "TitleE",
                table: "StaticContent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleA",
                table: "StaticContent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RouterLink",
                table: "StaticContent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "StaticContent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "StaticContent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "StaticContent",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "SortIndex",
                table: "SharedPrice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Point>(
                name: "Shape",
                table: "SharedPrice",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttachmentsLookUp",
                table: "AttachmentsLookUp",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentsLookUp_SharedPriceId",
                table: "AttachmentsLookUp",
                column: "SharedPriceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttachmentsLookUp",
                table: "AttachmentsLookUp");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentsLookUp_SharedPriceId",
                table: "AttachmentsLookUp");

            migrationBuilder.AlterColumn<string>(
                name: "TitleE",
                table: "StaticContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleA",
                table: "StaticContent",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "RouterLink",
                table: "StaticContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "StaticContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "StaticContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "StaticContent",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "SortIndex",
                table: "SharedPrice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "Shape",
                table: "SharedPrice",
                type: "geography",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttachmentsLookUp",
                table: "AttachmentsLookUp",
                columns: new[] { "SharedPriceId", "Id" });
        }
    }
}
