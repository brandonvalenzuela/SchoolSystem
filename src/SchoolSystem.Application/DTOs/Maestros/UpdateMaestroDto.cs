using SchoolSystem.Application.Common.Interfaces;
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
    /// DTO para actualizar la información de un Maestro existente.
    /// </summary>
    public class UpdateMaestroDto : IPersonaDto
    {
        [Required]
        public int Id { get; set; } // ID del registro Maestro

        [Required]
        public int UsuarioId { get; set; } // ID del registro Usuario

        // --- Campos actualizables del Usuario ---
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

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
        public bool Activo { get; set; } // Activo del Usuario

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime? FechaNacimiento { get; set; }


        // --- Campos actualizables del Maestro ---
        [Required(ErrorMessage = "El número de empleado es obligatorio.")]
        [StringLength(50)]
        public string NumeroEmpleado { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaBaja { get; set; }

        public TipoContrato? TipoContrato { get; set; }

        public EstatusLaboral? Estatus { get; set; }

        public decimal? Salario { get; set; }

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
