using SchoolSystem.Application.Common.Interfaces;
using SchoolSystem.Domain.Enums;
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
    public class CreatePadreDto : IPersonaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un alumno para vincular.")]
        public int AlumnoId { get; set; }

        // --- Datos de Usuario (Identidad) ---
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100)]
        public string ApellidoPaterno { get; set; }

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El email es vital para la activación de la cuenta.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Email { get; set; }

        public string? Telefono { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public Genero? Genero { get; set; }

        // --- Datos de Perfil de Padre ---
        public string? Relacion { get; set; } // Padre, Madre, Tutor...

        public string? Ocupacion { get; set; }

        public string? Puesto { get; set; }

        public string? LugarTrabajo { get; set; }

        public string? DireccionTrabajo { get; set; }

        public string? NivelEstudios { get; set; }

        public string? Carrera { get; set; }

        public string? EstadoCivil { get; set; }

        public string? Observaciones { get; set; }

        // --- Preferencias de Notificación ---
        public bool AceptaSMS { get; set; } = true;
        public bool AceptaEmail { get; set; } = true;
        public bool AceptaPush { get; set; } = true;

        // Nota: No incluimos Password ni Username aquí porque se 
        // generan automáticamente en el servicio durante el pre-registro.

    }
}
