using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCicloEscolarId_To_Grupos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CiclosEscolares_Escuelas_EscuelaId",
                table: "CiclosEscolares");

            migrationBuilder.DropIndex(
                name: "IX_CiclosEscolares_EscuelaId",
                table: "CiclosEscolares");

            migrationBuilder.AddColumn<int>(
                name: "CicloEscolarId",
                table: "Grupos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsActual",
                table: "CiclosEscolares",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_CicloEscolarId",
                table: "Grupos",
                column: "CicloEscolarId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_Escuela_CicloEscolarId",
                table: "Grupos",
                columns: new[] { "EscuelaId", "CicloEscolarId" });

            migrationBuilder.CreateIndex(
                name: "IX_CiclosEscolares_Escuela_Actual",
                table: "CiclosEscolares",
                columns: new[] { "EscuelaId", "EsActual" });

            migrationBuilder.CreateIndex(
                name: "IX_CiclosEscolares_Escuela_Clave_Unique",
                table: "CiclosEscolares",
                columns: new[] { "EscuelaId", "Clave" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CiclosEscolares_Escuelas_EscuelaId",
                table: "CiclosEscolares",
                column: "EscuelaId",
                principalTable: "Escuelas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_CiclosEscolares_CicloEscolarId",
                table: "Grupos",
                column: "CicloEscolarId",
                principalTable: "CiclosEscolares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Backfill
            migrationBuilder.Sql(@"
            UPDATE Grupos g
            JOIN CiclosEscolares c
              ON c.EscuelaId = g.EscuelaId
             AND c.Clave = g.CicloEscolar
            SET g.CicloEscolarId = c.Id
            WHERE g.CicloEscolarId IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CiclosEscolares_Escuelas_EscuelaId",
                table: "CiclosEscolares");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_CiclosEscolares_CicloEscolarId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_CicloEscolarId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_Escuela_CicloEscolarId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_CiclosEscolares_Escuela_Actual",
                table: "CiclosEscolares");

            migrationBuilder.DropIndex(
                name: "IX_CiclosEscolares_Escuela_Clave_Unique",
                table: "CiclosEscolares");

            migrationBuilder.DropColumn(
                name: "CicloEscolarId",
                table: "Grupos");

            migrationBuilder.AlterColumn<bool>(
                name: "EsActual",
                table: "CiclosEscolares",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CiclosEscolares_EscuelaId",
                table: "CiclosEscolares",
                column: "EscuelaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CiclosEscolares_Escuelas_EscuelaId",
                table: "CiclosEscolares",
                column: "EscuelaId",
                principalTable: "Escuelas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
