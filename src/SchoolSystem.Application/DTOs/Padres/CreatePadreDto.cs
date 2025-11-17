using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Padres
{
    /// <summary>
    /// DTO para la creación de un nuevo Padre/Tutor, incluyendo sus datos de Usuario.
    /// </summary>
    public class CreatePadreDto
    {
        [Required]
        public int EscuelaId { get; set; }

        // --- Campos para la entidad Usuario ---
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

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

        public DateTime? FechaNacimiento { get; set; }
        public Genero? Genero { get; set; }

        // --- Campos para la entidad Padre ---
        [StringLength(100)]
        public string Ocupacion { get; set; }

        [StringLength(200)]
        public string LugarTrabajo { get; set; }

        [StringLength(20)]
        public string TelefonoTrabajo { get; set; }

        [StringLength(300)]
        public string DireccionTrabajo { get; set; }

        [StringLength(100)]
        public string Puesto { get; set; }

        [StringLength(100)]
        public string NivelEstudios { get; set; }

        [StringLength(200)]
        public string Carrera { get; set; }

        [StringLength(50)]
        public string EstadoCivil { get; set; }
    }
}
