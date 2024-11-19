using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarSearchAPI.Migrations
{
    /// <inheritdoc />
    public partial class RentModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "surname",
                table: "applicationUsers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "applicationUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "licenceDate",
                table: "applicationUsers",
                newName: "LicenceDate");

            migrationBuilder.RenameColumn(
                name: "birthDate",
                table: "applicationUsers",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "applicationUsers",
                newName: "Email");

            migrationBuilder.CreateTable(
                name: "rents",
                columns: table => new
                {
                    RentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rents", x => x.RentId);
                    table.ForeignKey(
                        name: "FK_rents_applicationUsers_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "applicationUsers",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rents_UserEmail",
                table: "rents",
                column: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rents");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "applicationUsers",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "applicationUsers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LicenceDate",
                table: "applicationUsers",
                newName: "licenceDate");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "applicationUsers",
                newName: "birthDate");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "applicationUsers",
                newName: "email");
        }
    }
}
