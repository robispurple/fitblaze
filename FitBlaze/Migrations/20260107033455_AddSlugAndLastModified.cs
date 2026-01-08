using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitBlaze.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugAndLastModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Pages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Pages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Pages");
        }
    }
}
