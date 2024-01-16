using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commodity_Prices_Watcher.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBirthDateFieldToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterDatabase(
            //    collation: "Arabic_CI_AS");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "AspNetUsers");

            //migrationBuilder.AlterDatabase(
            //    oldCollation: "Arabic_CI_AS");
        }
    }
}
