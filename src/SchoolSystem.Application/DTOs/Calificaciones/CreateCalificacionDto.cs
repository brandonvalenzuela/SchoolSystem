using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    /// <summary>
    /// DTO para la creación de un nuevo registro de calificación.
    /// </summary>
    public class CreateCalificacionDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El ID del alumno es obligatorio.")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El ID de la materia es obligatorio.")]
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "El ID del grupo es obligatorio.")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "El ID del período es obligatorio.")]
        public int PeriodoId { get; set; }

        [Required(ErrorMessage = "La calificación numérica es obligatoria.")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal CalificacionNumerica { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del maestro que captura la calificación.")]
        public int CapturadoPor { get; set; }

        [StringLength(100, ErrorMessage = "El tipo de evaluación no puede exceder los 100 caracteres.")]
        public string TipoEvaluacion { get; set; }

        [Range(0, 100, ErrorMessage = "El peso debe estar entre 0 y 100.")]
        public decimal? Peso { get; set; }

        public string Observaciones { get; set; }
        public string Fortalezas { get; set; }
        public string AreasOportunidad { get; set; }
        public string Recomendaciones { get; set; }
    }
}
