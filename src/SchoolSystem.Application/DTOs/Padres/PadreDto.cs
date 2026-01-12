using SchoolSystem.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Padres
{
    /// <summary>
    /// DTO para representar la información combinada de un Padre y su Usuario asociado.
    /// </summary>
    public class PadreDto : IPersonaDto
    {
        public int Id { get; set; } // ID del registro Padre
        public int UsuarioId { get; set; } // ID del registro Usuario
        public int EscuelaId { get; set; }

        // --- Datos del Usuario ---

        public string Username { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string FotoUrl { get; set; }
        public bool Activo { get; set; }

        // --- Datos Específicos del Padre ---
        public string Ocupacion { get; set; }
        public string LugarTrabajo { get; set; }
        public string TelefonoTrabajo { get; set; }
        public string NivelEstudios { get; set; }
        public string EstadoCivil { get; set; }

        // --- Preferencias de Notificación ---
        public bool AceptaSMS { get; set; }
        public bool AceptaEmail { get; set; }
        public bool AceptaPush { get; set; }

        // --- Información Agregada ---
        /// <summary>
        /// Cantidad de hijos/tutelados asociados en la escuela.
        /// </summary>
        public int CantidadHijos { get; set; }

        /// <summary>
        /// Lista simplificada de los hijos asociados.
        /// </summary>
        public List<HijoAsociadoDto> Hijos { get; set; }
 
    }

    /// <summary>
    /// DTO simple para representar un hijo asociado a un padre.
    /// </summary>
    public class HijoAsociadoDto
    {
        public int AlumnoId { get; set; }
        public string NombreCompletoAlumno { get; set; }
        public string Matricula { get; set; }
    }
}
