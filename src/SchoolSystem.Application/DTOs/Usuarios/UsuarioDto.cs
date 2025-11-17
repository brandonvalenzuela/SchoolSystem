using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Usuarios
{
    /// <summary>
    /// DTO para representar la información de un usuario al ser leída.
    /// </summary>
    public class UsuarioDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Rol del usuario (ej: "Director", "Maestro", "Padre").
        /// </summary>
        public string Rol { get; set; }

        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string NombreCompleto { get; set; }

        public string Telefono { get; set; }
        public string FotoUrl { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        /// <summary>
        /// Edad del usuario.
        /// </summary>
        public int? Edad { get; set; }

        /// <summary>
        /// Género del usuario (ej: "Masculino", "Femenino").
        /// </summary>
        public string Genero { get; set; }

        public bool Activo { get; set; }
        public DateTime? UltimoAcceso { get; set; }
    }
}
