using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations.SchoolSystemDb
{
    /// <inheritdoc />
    public partial class CreateCalificacionAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Materias_Escuela_Nombre",
                table: "Materias");

            migrationBuilder.CreateTable(
                name: "CalificacionesAuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EscuelaId = table.Column<int>(type: "int", nullable: false),
                    CalificacionId = table.Column<int>(type: "int", nullable: false),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    PeriodoId = table.Column<int>(type: "int", nullable: false),
                    CalificacionAnterior = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    CalificacionNueva = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    ObservacionesAnteriores = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ObservacionesNuevas = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Motivo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecalificadoPor = table.Column<int>(type: "int", nullable: false),
                    RecalificadoAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Origen = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CorrelationId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalificacionesAuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalificacionesAuditLog_Calificaciones_CalificacionId",
                        column: x => x.CalificacionId,
                        principalTable: "Calificaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalificacionesAuditLog_Escuelas_EscuelaId",
                        column: x => x.EscuelaId,
                        principalTable: "Escuelas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Escuela_Nombre",
                table: "Materias",
                columns: new[] { "EscuelaId", "Nombre" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_CorrelationId",
                table: "CalificacionesAuditLog",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Escuela_Alumno_Periodo",
                table: "CalificacionesAuditLog",
                columns: new[] { "EscuelaId", "AlumnoId", "PeriodoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Escuela_Calificacion",
                table: "CalificacionesAuditLog",
                columns: new[] { "EscuelaId", "CalificacionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Escuela_Grupo_Periodo",
                table: "CalificacionesAuditLog",
                columns: new[] { "EscuelaId", "GrupoId", "PeriodoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Escuela_Materia_Periodo",
                table: "CalificacionesAuditLog",
                columns: new[] { "EscuelaId", "MateriaId", "PeriodoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Escuela_RecalificadoPor",
                table: "CalificacionesAuditLog",
                columns: new[] { "EscuelaId", "RecalificadoPor" });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_RecalificadoAtUtc",
                table: "CalificacionesAuditLog",
                column: "RecalificadoAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CalificacionesAuditLog_CalificacionId",
                table: "CalificacionesAuditLog",
                column: "CalificacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalificacionesAuditLog");

            migrationBuilder.DropIndex(
                name: "IX_Materias_Escuela_Nombre",
                table: "Materias");

            migrationBuilder.CreateIndex(
                name: "UX_Materias_Escuela_Nombre",
                table: "Materias",
                columns: new[] { "EscuelaId", "Nombre" },
                unique: true);
        }
    }
}
