using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentState",
                table: "Rents");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Models",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "Name" },
                values: new object[,]
                {
                    { 4, "BMW" },
                    { 5, "Audi" },
                    { 6, "Toyota" },
                    { 7, "Mercedes-Benz" },
                    { 8, "Volkswagen" },
                    { 9, "Hyundai" },
                    { 10, "Nissan" }
                });

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 1,
                column: "Location",
                value: "Warsaw");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 2,
                columns: new[] { "IsActive", "Location" },
                values: new object[] { true, "Krakow" });

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 3,
                column: "Location",
                value: "Bydgoszcz");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 4,
                column: "Location",
                value: "Warsaw");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 5,
                columns: new[] { "IsActive", "Location", "ModelId" },
                values: new object[] { true, "Krakow", 3 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "IsActive", "Location", "Mileage", "ModelId" },
                values: new object[,]
                {
                    { 6, true, "Bydgoszcz", 8000, 4 },
                    { 7, true, "Warsaw", 15000, 4 }
                });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                column: "Year",
                value: null);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 2,
                column: "Year",
                value: null);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 3,
                columns: new[] { "BasePrice", "BrandId", "Name", "Year" },
                values: new object[] { 900, 1, "Model 3", null });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 4,
                columns: new[] { "BasePrice", "BrandId", "Name" },
                values: new object[] { 800, 2, "Mustang" });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BasePrice", "BrandId", "Name", "Year" },
                values: new object[,]
                {
                    { 5, 700, 2, "F-150", null },
                    { 6, 900, 3, "Camaro", null },
                    { 7, 950, 3, "Silverado", "2024" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "IsActive", "Location", "Mileage", "ModelId" },
                values: new object[,]
                {
                    { 8, true, "Krakow", 20000, 5 },
                    { 9, true, "Bydgoszcz", 7000, 5 },
                    { 10, true, "Warsaw", 500, 6 },
                    { 11, true, "Krakow", 11000, 7 },
                    { 12, true, "Bydgoszcz", 13000, 7 }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BasePrice", "BrandId", "Name", "Year" },
                values: new object[,]
                {
                    { 8, 950, 4, "X5", null },
                    { 9, 1050, 4, "3 Series", "2023" },
                    { 10, 1000, 5, "A4", null },
                    { 11, 1100, 5, "A6", null },
                    { 12, 700, 6, "Corolla", null },
                    { 13, 800, 6, "Camry", "2023" },
                    { 14, 1300, 7, "C-Class", null },
                    { 15, 1500, 7, "E-Class", "2024" },
                    { 16, 650, 8, "Golf", null },
                    { 17, 750, 8, "Passat", "2023" },
                    { 18, 600, 9, "Elantra", null },
                    { 19, 750, 9, "Sonata", "2022" },
                    { 20, 750, 10, "Altima", null },
                    { 21, 680, 10, "Sentra", "2023" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "IsActive", "Location", "Mileage", "ModelId" },
                values: new object[,]
                {
                    { 13, true, "Warsaw", 18000, 8 },
                    { 14, true, "Krakow", 7500, 8 },
                    { 15, true, "Bydgoszcz", 6200, 9 },
                    { 16, true, "Warsaw", 4000, 9 },
                    { 17, true, "Krakow", 14000, 10 },
                    { 18, true, "Bydgoszcz", 6000, 10 },
                    { 19, true, "Warsaw", 9200, 11 },
                    { 20, true, "Krakow", 10500, 11 },
                    { 21, true, "Bydgoszcz", 8900, 12 },
                    { 22, true, "Warsaw", 10000, 12 },
                    { 23, true, "Krakow", 4300, 13 },
                    { 24, true, "Bydgoszcz", 5100, 13 },
                    { 25, true, "Warsaw", 2200, 14 },
                    { 26, true, "Krakow", 14500, 15 },
                    { 27, true, "Bydgoszcz", 7500, 16 },
                    { 28, true, "Warsaw", 14000, 17 },
                    { 29, true, "Krakow", 9300, 18 },
                    { 30, true, "Bydgoszcz", 6200, 18 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Cars");

            migrationBuilder.AddColumn<short>(
                name: "RentState",
                table: "Rents",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Models",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 2,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 5,
                columns: new[] { "IsActive", "ModelId" },
                values: new object[] { false, 4 });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                column: "Year",
                value: "2024");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 2,
                column: "Year",
                value: "2024");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 3,
                columns: new[] { "BasePrice", "BrandId", "Name", "Year" },
                values: new object[] { 800, 2, "Mustang", "2023" });

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 4,
                columns: new[] { "BasePrice", "BrandId", "Name" },
                values: new object[] { 900, 3, "Camaro" });
        }
    }
}
