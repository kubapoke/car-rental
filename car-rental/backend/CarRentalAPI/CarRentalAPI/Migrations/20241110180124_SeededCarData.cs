using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeededCarData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelID",
                table: "Models",
                newName: "ModelId");

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "Name" },
                values: new object[,]
                {
                    { 1, "Tesla" },
                    { 2, "Ford" },
                    { 3, "Chevrolet" }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BasePrice", "BrandId", "Name", "Year" },
                values: new object[,]
                {
                    { 1, 1000, 1, "Model S", "2024" },
                    { 2, 1500, 1, "Model X", "2024" },
                    { 3, 800, 2, "Mustang", "2023" },
                    { 4, 900, 3, "Camaro", "2023" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "IsActive", "Mileage", "ModelId" },
                values: new object[,]
                {
                    { 1, true, 15000, 1 },
                    { 2, false, 12000, 1 },
                    { 3, true, 5000, 2 },
                    { 4, true, 3000, 3 },
                    { 5, false, 2500, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Models",
                newName: "ModelID");
        }
    }
}
