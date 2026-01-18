using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_Cargos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "Cargos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_CicloEscolarId",
                table: "Cargos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Escuela_CicloEscolarId",
                table: "Cargos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_CiclosEscolares_CicloEscolarId",
                table: "Cargos",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
UPDATE Cargos ca
JOIN CiclosEscolares c
  ON c.EscuelaId = ca.EscuelaId
 AND c.Clave = ca.CicloEscolar
SET ca.CicloEscolarId = c.Id
WHERE ca.CicloEscolarId IS NULL;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_CiclosEscolares_CicloEscolarId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_CicloEscolarId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_Escuela_CicloEscolarId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "Cargos");
        }
    }
}
