using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Grupos
{
    /// <summary>
    /// DTO para representar la información detallada de un grupo al ser leído.
    /// </summary>
    public class GrupoDto
    {
        public int Id { get; set; }
        public int GradoId { get; set; }
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre descriptivo del grado y nivel (ej: "3° de Primaria").
        /// </summary>
        public string NombreGrado { get; set; }

        /// <summary>
        /// Nombre del grupo (ej: "A", "B").
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Nombre completo del grupo (ej: "3° de Primaria - Grupo A").
        /// </summary>
        public string NombreCompleto { get; set; }

        /// <summary>
        /// Ciclo escolar al que pertenece (ej: "2024-2025").
        /// </summary>
        public string CicloEscolar { get; set; }

        public int CapacidadMaxima { get; set; }

        /// <summary>
        /// Cantidad actual de alumnos inscritos.
        /// </summary>
        public int CantidadAlumnos { get; set; }

        /// <summary>
        /// ID del maestro titular (si tiene).
        /// </summary>
        public int? MaestroTitularId { get; set; }

        /// <summary>
        /// Nombre del maestro titular (si tiene).
        /// </summary>
        public string NombreMaestroTitular { get; set; }

        /// <summary>
        /// Aula asignada (ej: "Salón 101").
        /// </summary>
        public string Aula { get; set; }

        /// <summary>
        /// Turno del grupo (ej: "Matutino").
        /// </summary>
        public string Turno { get; set; }

        public bool Activo { get; set; }
    }
}
