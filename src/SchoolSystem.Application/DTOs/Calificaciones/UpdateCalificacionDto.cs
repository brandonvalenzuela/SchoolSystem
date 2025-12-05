using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    /// <summary>
    /// DTO para actualizar un registro de calificación existente.
    /// </summary>
    public class UpdateCalificacionDto
    {
        [Required]
        public int Id { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal? CalificacionNumerica { get; set; }

        [StringLength(100)]
        public string? TipoEvaluacion { get; set; }

        [Range(0, 100)]
        public decimal? Peso { get; set; }

        public string? Observaciones { get; set; }
        public string? Fortalezas { get; set; }
        public string? AreasOportunidad { get; set; }
        public string? Recomendaciones { get; set; }

        // Campos para auditoría de la modificación
        [Required(ErrorMessage = "Se requiere el motivo de la modificación.")]
        [StringLength(500)]
        public required string MotivoModificacion { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del usuario que modifica.")]
        public int ModificadoPor { get; set; }
    }
}
