using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSearchAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCarRentalIdBackToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RentalCompanyRentId",
                table: "rents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RentalCompanyRentId",
                table: "rents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
