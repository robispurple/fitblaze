using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitBlaze.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WikiPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Version = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WikiPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WikiPages_WikiPages_ParentId",
                        column: x => x.ParentId,
                        principalTable: "WikiPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WikiPages_CreatedDate",
                table: "WikiPages",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_WikiPages_IsPublished",
                table: "WikiPages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_WikiPages_ModifiedDate",
                table: "WikiPages",
                column: "ModifiedDate");

            migrationBuilder.CreateIndex(
                name: "IX_WikiPages_ParentId",
                table: "WikiPages",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WikiPages_Slug",
                table: "WikiPages",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WikiPages");
        }
    }
}
