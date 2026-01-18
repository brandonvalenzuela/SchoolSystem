using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_EstadosCuenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "EstadosCuenta",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_CicloEscolarId",
                table: "EstadosCuenta",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuenta_Escuela_CicloEscolarId",
                table: "EstadosCuenta",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EstadosCuenta_CiclosEscolares_CicloEscolarId",
                table: "EstadosCuenta",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Backfill
            migrationBuilder.Sql(@"
UPDATE EstadosCuenta ec
JOIN CiclosEscolares c
  ON c.EscuelaId = ec.EscuelaId
 AND c.Clave = ec.CicloEscolar
SET ec.CicloEscolarId = c.Id
WHERE ec.CicloEscolarId IS NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstadosCuenta_CiclosEscolares_CicloEscolarId",
                table: "EstadosCuenta");

            migrationBuilder.DropIndex(
                name: "IX_EstadosCuenta_CicloEscolarId",
                table: "EstadosCuenta");

            migrationBuilder.DropIndex(
                name: "IX_EstadosCuenta_Escuela_CicloEscolarId",
                table: "EstadosCuenta");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "EstadosCuenta");
        }
    }
}
