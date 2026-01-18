using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_ConceptosPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "ConceptosPago",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_CicloEscolarId",
                table: "ConceptosPago",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPago_Escuela_CicloEscolarId",
                table: "ConceptosPago",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ConceptosPago_CiclosEscolares_CicloEscolarId",
                table: "ConceptosPago",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Backfill
            migrationBuilder.Sql(@"
UPDATE ConceptosPago cp
JOIN CiclosEscolares c
  ON c.EscuelaId = cp.EscuelaId
 AND c.Clave = cp.CicloEscolar
SET cp.CicloEscolarId = c.Id
WHERE cp.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConceptosPago_CiclosEscolares_CicloEscolarId",
                table: "ConceptosPago");

            migrationBuilder.DropIndex(
                name: "IX_ConceptosPago_CicloEscolarId",
                table: "ConceptosPago");

            migrationBuilder.DropIndex(
                name: "IX_ConceptosPago_Escuela_CicloEscolarId",
                table: "ConceptosPago");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "ConceptosPago");
        }
    }
}
