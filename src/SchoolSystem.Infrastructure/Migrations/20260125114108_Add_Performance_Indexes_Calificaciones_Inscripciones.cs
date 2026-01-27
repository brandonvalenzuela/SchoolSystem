using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Performance_Indexes_Calificaciones_Inscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Grupo_Ciclo_Estatus_Alumno",
                table: "Inscripciones",
                columns: new[] { "GrupoId", "CicloEscolarId", "Estatus", "AlumnoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_Grupo_Alumno",
                table: "Calificaciones",
                columns: new[] { "GrupoId", "AlumnoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_Grupo_Materia_Periodo_Alumno",
                table: "Calificaciones",
                columns: new[] { "GrupoId", "MateriaId", "PeriodoId", "AlumnoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inscripciones_Grupo_Ciclo_Estatus_Alumno",
                table: "Inscripciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_Grupo_Alumno",
                table: "Calificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_Grupo_Materia_Periodo_Alumno",
                table: "Calificaciones");
        }
    }
}
