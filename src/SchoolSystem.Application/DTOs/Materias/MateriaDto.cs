using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Materias
{
    /// <summary>
    /// DTO para representar la información detallada de una materia al ser leída.
    /// </summary>
    public class MateriaDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre de la materia (ej: "Matemáticas III").
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Clave o código de la materia (ej: "MAT-301").
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Área académica a la que pertenece (ej: "Ciencias Exactas").
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Tipo de materia (ej: "Teórica", "Laboratorio").
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Nombre del icono para la UI (ej: "calculate").
        /// </summary>
        public string Icono { get; set; }

        /// <summary>
        /// Color representativo en formato hexadecimal (ej: "#FF5733").
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Indica si la materia está activa.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Cantidad de grupos donde se imparte actualmente.
        /// </summary>
        public int CantidadGrupos { get; set; }
    }
}
