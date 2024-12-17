using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSearchAPI.Migrations
{
    /// <inheritdoc />
    public partial class NameAbdRetalIdAAddedToRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RentalCompanyName",
                table: "rents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RentalCompanyRentId",
                table: "rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "rents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalCompanyName",
                table: "rents");

            migrationBuilder.DropColumn(
                name: "RentalCompanyRentId",
                table: "rents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "rents");
        }
    }
}
