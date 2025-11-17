using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Maestros
{
    /// <summary>
    /// DTO para representar la información combinada de un Maestro y su Usuario asociado.
    /// </summary>
    public class MaestroDto
    {
        public int Id { get; set; } // ID del registro Maestro
        public int UsuarioId { get; set; } // ID del registro Usuario
        public int EscuelaId { get; set; }

        // --- Datos del Usuario ---
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string FotoUrl { get; set; }
        public bool Activo { get; set; }

        // --- Datos Específicos del Maestro ---
        public string NumeroEmpleado { get; set; }
        public string Especialidad { get; set; }
        public string TituloAcademico { get; set; }
        public string Universidad { get; set; }
        public DateTime? FechaIngreso { get; set; }

        /// <summary>
        /// Estatus laboral del maestro (ej: "Activo", "Licencia").
        /// </summary>
        public string Estatus { get; set; }

        // --- Información Agregada ---
        /// <summary>
        /// Cantidad de materias que imparte actualmente.
        /// </summary>
        public int CantidadMaterias { get; set; }

        /// <summary>
        /// Antigüedad en años dentro de la institución.
        /// </summary>
        public int? AntiguedadEnAnios { get; set; }
    }
}
