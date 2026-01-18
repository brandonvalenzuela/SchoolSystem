using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_PeriodosEvaluacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "PeriodosEvaluacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_CicloEscolarId",
                table: "PeriodosEvaluacion",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosEvaluacion_Escuela_CicloEscolarId",
                table: "PeriodosEvaluacion",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodosEvaluacion_CiclosEscolares_CicloEscolarId",
                table: "PeriodosEvaluacion",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Backfill
            migrationBuilder.Sql(@"
UPDATE PeriodosEvaluacion p
JOIN CiclosEscolares c
  ON c.EscuelaId = p.EscuelaId
 AND c.Clave = p.CicloEscolar
SET p.CicloEscolarId = c.Id
WHERE p.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeriodosEvaluacion_CiclosEscolares_CicloEscolarId",
                table: "PeriodosEvaluacion");

            migrationBuilder.DropIndex(
                name: "IX_PeriodosEvaluacion_CicloEscolarId",
                table: "PeriodosEvaluacion");

            migrationBuilder.DropIndex(
                name: "IX_PeriodosEvaluacion_Escuela_CicloEscolarId",
                table: "PeriodosEvaluacion");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "PeriodosEvaluacion");
        }
    }
}
