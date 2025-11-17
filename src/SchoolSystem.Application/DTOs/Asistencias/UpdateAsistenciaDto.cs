using SchoolSystem.Domain.Enums.Asistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{
    /// <summary>
    /// DTO para el registro masivo de asistencias de un grupo.
    /// </summary>
    public class UpdateAsistenciaDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El estatus es obligatorio.")]
        public EstadoAsistencia Estatus { get; set; }

        public bool Justificado { get; set; }

        [StringLength(1000)]
        public string Motivo { get; set; }

        [Url]
        [StringLength(500)]
        public string JustificanteUrl { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del usuario que aprueba la justificación.")]
        public int AproboJustificanteId { get; set; }

        [StringLength(1000)]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del usuario que realiza la modificación.")]
        public int UsuarioModificoId { get; set; }
    }
}
