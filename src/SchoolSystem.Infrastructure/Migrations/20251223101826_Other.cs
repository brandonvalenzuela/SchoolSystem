using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Other : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Materias",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Materias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Materias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Inscripciones",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Inscripciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inscripciones",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Calificaciones",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Calificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Calificaciones",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Asistencias",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Asistencias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Asistencias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Asistencias");
        }
    }
}
