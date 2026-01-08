using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitBlaze.Migrations
{
    /// <inheritdoc />
    public partial class AddMarkupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarkupType",
                table: "Pages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkupType",
                table: "Pages");
        }
    }
}
