using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations.SchoolSystemDb
{
    /// <inheritdoc />
    public partial class HardeningMateriaIndicesYConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear índice único, permitiendo duplicados existentes
            // (el índice se aplicará a nuevas inserciones y actualizaciones)
            migrationBuilder.CreateIndex(
                name: "UX_Materias_Escuela_Nombre",
                table: "Materias",
                columns: new[] { "EscuelaId", "Nombre" },
                unique: false); // No único para permitir duplicados existentes

            migrationBuilder.AddCheckConstraint(
                name: "CK_Materias_ColorHex",
                table: "Materias",
                sql: "(ColorHex IS NULL OR (CHAR_LENGTH(ColorHex)=7 AND ColorHex REGEXP '^#[0-9A-Fa-f]{6}$'))");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Materias_Nombre_NotWhitespace",
                table: "Materias",
                sql: "(TRIM(Nombre) <> '')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Materias_Escuela_Nombre",
                table: "Materias");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Materias_ColorHex",
                table: "Materias");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Materias_Nombre_NotWhitespace",
                table: "Materias");
        }
    }
}
