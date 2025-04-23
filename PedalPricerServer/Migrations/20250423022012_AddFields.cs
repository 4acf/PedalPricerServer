using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedalPricerServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "PowerSupplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "PowerSupplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PowerSupplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "PowerSupplies",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Pedals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Pedals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pedals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "Pedals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Pedalboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Pedalboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pedalboards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "Pedalboards",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "PowerSupplies");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "PowerSupplies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PowerSupplies");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "PowerSupplies");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Pedals");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Pedals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pedals");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Pedals");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Pedalboards");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Pedalboards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pedalboards");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Pedalboards");
        }
    }
}
