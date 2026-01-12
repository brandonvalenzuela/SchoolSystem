using SchoolSystem.Application.Common.Interfaces;
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
    /// DTO para actualizar la información de un Padre/Tutor existente.
    /// </summary>
    public class UpdatePadreDto : IPersonaDto
    {
        [Required]
        public int Id { get; set; } // ID del registro Padre

        [Required]
        public int UsuarioId { get; set; } // ID del registro Usuario

        // --- Campos actualizables del Usuario ---
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, MinimumLength = 4)]
        public string? Username { get; set; }

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

        public DateTime? FechaNacimiento { get; set; }
        public Genero? Genero { get; set; }
        public bool Activo { get; set; }

        // --- Campos actualizables del Padre ---
        [StringLength(100)]
        public string? Ocupacion { get; set; }

        [StringLength(200)]
        public string? LugarTrabajo { get; set; }

        [StringLength(20)]
        public string? TelefonoTrabajo { get; set; }

        [StringLength(300)]
        public string? DireccionTrabajo { get; set; }

        [StringLength(100)]
        public string? Puesto { get; set; }

        [StringLength(100)]
        public string? NivelEstudios { get; set; }

        [StringLength(200)]
        public string? Carrera { get; set; }

        [StringLength(50)]
        public string? EstadoCivil { get; set; }

        // --- Preferencias de Notificación ---
        public bool AceptaSMS { get; set; }
        public bool AceptaEmail { get; set; }
        public bool AceptaPush { get; set; }
    }
}
