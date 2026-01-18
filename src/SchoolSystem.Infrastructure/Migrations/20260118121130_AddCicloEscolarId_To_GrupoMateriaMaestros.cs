using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_GrupoMateriaMaestros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "GrupoMateriaMaestros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_CicloEscolarId",
                table: "GrupoMateriaMaestros",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMateriaMaestros_Escuela_CicloEscolarId",
                table: "GrupoMateriaMaestros",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GrupoMateriaMaestros_CiclosEscolares_CicloEscolarId",
                table: "GrupoMateriaMaestros",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.Sql(@"
UPDATE GrupoMateriaMaestros gmm
JOIN CiclosEscolares c
  ON c.EscuelaId = gmm.EscuelaId
 AND c.Clave = gmm.CicloEscolar
SET gmm.CicloEscolarId = c.Id
WHERE gmm.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrupoMateriaMaestros_CiclosEscolares_CicloEscolarId",
                table: "GrupoMateriaMaestros");

            migrationBuilder.DropIndex(
                name: "IX_GrupoMateriaMaestros_CicloEscolarId",
                table: "GrupoMateriaMaestros");

            migrationBuilder.DropIndex(
                name: "IX_GrupoMateriaMaestros_Escuela_CicloEscolarId",
                table: "GrupoMateriaMaestros");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "GrupoMateriaMaestros");
        }
    }
}
