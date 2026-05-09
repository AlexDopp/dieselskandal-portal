using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapp_Dieselskandal.Migrations
{
    /// <inheritdoc />
    public partial class AddAuftragZusatzfelder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AndereKanzleiBeauftragt",
                table: "Auftraege",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FahrzeugInBesitz",
                table: "Auftraege",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "KlageDatum",
                table: "Auftraege",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KlageEingereicht",
                table: "Auftraege",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AndereKanzleiBeauftragt",
                table: "Auftraege");

            migrationBuilder.DropColumn(
                name: "FahrzeugInBesitz",
                table: "Auftraege");

            migrationBuilder.DropColumn(
                name: "KlageDatum",
                table: "Auftraege");

            migrationBuilder.DropColumn(
                name: "KlageEingereicht",
                table: "Auftraege");
        }
    }
}
