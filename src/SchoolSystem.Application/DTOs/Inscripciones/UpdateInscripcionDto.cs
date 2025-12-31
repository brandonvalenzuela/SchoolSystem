using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Inscripciones
{
    /// <summary>
    /// DTO para actualizar una inscripción existente.
    /// </summary>
    public class UpdateInscripcionDto
    {
        [Required]
        public int Id { get; set; }

        // No se permite cambiar el alumno de una inscripción, se crearía una nueva.
        // No se permite cambiar el ciclo escolar.

        [Required(ErrorMessage = "El ID del grupo es obligatorio.")]
        public int GrupoId { get; set; }

        [Required]
        public EstatusInscripcion Estatus { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Range(1, 100)]
        public int? NumeroLista { get; set; }

        public bool Becado { get; set; }

        [StringLength(100)]
        public string? TipoBeca { get; set; }

        [Range(0, 100)]
        public decimal? PorcentajeBeca { get; set; }

        public bool Repetidor { get; set; }

        [StringLength(1000)]
        public string? Observaciones { get; set; }

        // Campos específicos para bajas
        [StringLength(500)]
        public string? MotivoBaja { get; set; }

        // Campos específicos para cambio de grupo
        [StringLength(500)]
        public string? MotivoCambioGrupo { get; set; }
    }
}
