using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Enums.Escuelas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Usuarios
{
    /// <summary>
    /// DTO para la creación de un nuevo usuario.
    /// </summary>
    public class CreateUsuarioDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 100 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; } // La contraseña en texto plano, para ser hasheada.

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public RolUsuario Rol { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoPaterno { get; set; }

        [StringLength(100)]
        public string ApellidoMaterno { get; set; }

        [Phone]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Url]
        [StringLength(500)]
        public string FotoUrl { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public Genero? Genero { get; set; }

        [Phone]
        [StringLength(20)]
        public string TelefonoEmergencia { get; set; }
    }
}
