using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCalificacionObservacionesOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Calificaciones",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Calificaciones",
                keyColumn: "Observaciones",
                keyValue: null,
                column: "Observaciones",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Calificaciones",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
