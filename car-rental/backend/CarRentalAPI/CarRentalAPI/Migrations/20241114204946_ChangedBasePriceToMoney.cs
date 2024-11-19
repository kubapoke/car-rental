using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBasePriceToMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BasePrice",
                table: "Models",
                type: "money",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                column: "BasePrice",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 2,
                column: "BasePrice",
                value: 1500m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 3,
                column: "BasePrice",
                value: 900m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 4,
                column: "BasePrice",
                value: 800m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 5,
                column: "BasePrice",
                value: 700m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 6,
                column: "BasePrice",
                value: 900m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 7,
                column: "BasePrice",
                value: 950m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 8,
                column: "BasePrice",
                value: 950m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 9,
                column: "BasePrice",
                value: 1050m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 10,
                column: "BasePrice",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 11,
                column: "BasePrice",
                value: 1100m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 12,
                column: "BasePrice",
                value: 700m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 13,
                column: "BasePrice",
                value: 800m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 14,
                column: "BasePrice",
                value: 1300m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 15,
                column: "BasePrice",
                value: 1500m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 16,
                column: "BasePrice",
                value: 650m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 17,
                column: "BasePrice",
                value: 750m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 18,
                column: "BasePrice",
                value: 600m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 19,
                column: "BasePrice",
                value: 750m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 20,
                column: "BasePrice",
                value: 750m);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 21,
                column: "BasePrice",
                value: 680m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BasePrice",
                table: "Models",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                column: "BasePrice",
                value: 1000);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 2,
                column: "BasePrice",
                value: 1500);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 3,
                column: "BasePrice",
                value: 900);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 4,
                column: "BasePrice",
                value: 800);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 5,
                column: "BasePrice",
                value: 700);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 6,
                column: "BasePrice",
                value: 900);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 7,
                column: "BasePrice",
                value: 950);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 8,
                column: "BasePrice",
                value: 950);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 9,
                column: "BasePrice",
                value: 1050);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 10,
                column: "BasePrice",
                value: 1000);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 11,
                column: "BasePrice",
                value: 1100);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 12,
                column: "BasePrice",
                value: 700);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 13,
                column: "BasePrice",
                value: 800);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 14,
                column: "BasePrice",
                value: 1300);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 15,
                column: "BasePrice",
                value: 1500);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 16,
                column: "BasePrice",
                value: 650);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 17,
                column: "BasePrice",
                value: 750);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 18,
                column: "BasePrice",
                value: 600);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 19,
                column: "BasePrice",
                value: 750);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 20,
                column: "BasePrice",
                value: 750);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 21,
                column: "BasePrice",
                value: 680);
        }
    }
}
