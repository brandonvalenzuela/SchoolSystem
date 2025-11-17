using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Grados
{
    /// <summary>
    /// DTO para representar la información detallada de un grado al ser leído.
    /// </summary>
    public class GradoDto
    {
        public int Id { get; set; }
        public int NivelEducativoId { get; set; }
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre del nivel educativo al que pertenece (ej: "Primaria").
        /// </summary>
        public string NombreNivelEducativo { get; set; }

        /// <summary>
        /// Nombre del grado (ej: "1°", "A").
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Nombre completo del grado (ej: "1° de Primaria").
        /// </summary>
        public string NombreCompleto { get; set; }

        /// <summary>
        /// Orden del grado dentro del nivel.
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Indica si el grado está activo.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Cantidad de grupos activos en este grado.
        /// </summary>
        public int CantidadGruposActivos { get; set; }

        /// <summary>
        /// Cantidad de materias asignadas a este grado.
        /// </summary>
        public int CantidadMaterias { get; set; }
    }
}
