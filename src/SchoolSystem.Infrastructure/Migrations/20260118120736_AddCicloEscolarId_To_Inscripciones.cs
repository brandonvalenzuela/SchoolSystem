using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_Inscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "Inscripciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_CicloEscolarId",
                table: "Inscripciones",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Escuela_CicloEscolarId",
                table: "Inscripciones",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripciones_CiclosEscolares_CicloEscolarId",
                table: "Inscripciones",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Backfill desde string (CicloEscolar) hacia FK (CicloEscolarId)
            migrationBuilder.Sql(@"
UPDATE Inscripciones i
JOIN CiclosEscolares c
  ON c.EscuelaId = i.EscuelaId
 AND c.Clave = i.CicloEscolar
SET i.CicloEscolarId = c.Id
WHERE i.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripciones_CiclosEscolares_CicloEscolarId",
                table: "Inscripciones");

            migrationBuilder.DropIndex(
                name: "IX_Inscripciones_CicloEscolarId",
                table: "Inscripciones");

            migrationBuilder.DropIndex(
                name: "IX_Inscripciones_Escuela_CicloEscolarId",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "Inscripciones");
        }
    }
}
