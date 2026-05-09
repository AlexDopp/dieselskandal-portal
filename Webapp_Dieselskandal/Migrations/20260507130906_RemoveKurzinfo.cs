using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapp_Dieselskandal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKurzinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kurzinfo",
                table: "Auftraege");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kurzinfo",
                table: "Auftraege",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
