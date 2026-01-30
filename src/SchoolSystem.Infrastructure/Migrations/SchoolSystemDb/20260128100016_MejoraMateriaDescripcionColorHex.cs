using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations.SchoolSystemDb
{
    /// <inheritdoc />
    public partial class MejoraMateriaDescripcionColorHex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Materias");

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "Materias",
                type: "varchar(7)",
                maxLength: 7,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "Materias");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Materias",
                type: "varchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
