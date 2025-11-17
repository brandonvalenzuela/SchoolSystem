using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Maestros
{
    /// <summary>
    /// DTO para la creación de un nuevo Maestro, incluyendo sus datos de Usuario.
    /// </summary>
    public class CreateMaestroDto
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

        // --- Campos para la entidad Maestro ---
        [Required(ErrorMessage = "El número de empleado es obligatorio.")]
        [StringLength(50)]
        public string NumeroEmpleado { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        public TipoContrato? TipoContrato { get; set; }

        [StringLength(50)]
        public string CedulaProfesional { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(200)]
        public string Especialidad { get; set; }

        [Required(ErrorMessage = "El título académico es obligatorio.")]
        [StringLength(100)]
        public string TituloAcademico { get; set; }

        [StringLength(200)]
        public string Universidad { get; set; }

        [Range(1950, 2100)]
        public int? AñoGraduacion { get; set; }

        [Range(0, 50)]
        public int? AñosExperiencia { get; set; }
    }
}
