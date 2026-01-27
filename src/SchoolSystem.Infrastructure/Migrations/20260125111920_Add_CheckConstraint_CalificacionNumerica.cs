using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CheckConstraint_CalificacionNumerica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calificacion_Alumno_Materia_Periodo",
                table: "Calificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Alumno_Materia_Periodo",
                table: "Calificaciones",
                columns: new[] { "AlumnoId", "MateriaId", "PeriodoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calificacion_Alumno_Materia_Periodo",
                table: "Calificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Alumno_Materia_Periodo",
                table: "Calificaciones",
                columns: new[] { "AlumnoId", "MateriaId", "PeriodoId" },
                unique: true);
        }
    }
}
