using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_AlumnoPuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "AlumnoPuntos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_CicloEscolarId",
                table: "AlumnoPuntos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoPuntos_Escuela_CicloEscolarId",
                table: "AlumnoPuntos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoPuntos_CiclosEscolares_CicloEscolarId",
                table: "AlumnoPuntos",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
UPDATE AlumnoPuntos ap
JOIN CiclosEscolares c
  ON c.EscuelaId = ap.EscuelaId
 AND c.Clave = ap.CicloEscolar
SET ap.CicloEscolarId = c.Id
WHERE ap.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoPuntos_CiclosEscolares_CicloEscolarId",
                table: "AlumnoPuntos");

            migrationBuilder.DropIndex(
                name: "IX_AlumnoPuntos_CicloEscolarId",
                table: "AlumnoPuntos");

            migrationBuilder.DropIndex(
                name: "IX_AlumnoPuntos_Escuela_CicloEscolarId",
                table: "AlumnoPuntos");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "AlumnoPuntos");
        }
    }
}
