using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Inscripciones
{
    /// <summary>
    /// DTO para la creación de una nueva inscripción.
    /// </summary>
    public class CreateInscripcionDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El ID del alumno a inscribir es obligatorio.")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El ID del grupo es obligatorio.")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "El ciclo escolar es obligatorio.")]
        [StringLength(20)]
        public string CicloEscolar { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Range(1, 100, ErrorMessage = "El número de lista debe ser un número positivo.")]
        public int? NumeroLista { get; set; }

        public bool Becado { get; set; } = false;

        [StringLength(100)]
        public string TipoBeca { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de beca debe estar entre 0 y 100.")]
        public decimal? PorcentajeBeca { get; set; }

        public bool Repetidor { get; set; } = false;

        [StringLength(1000)]
        public string Observaciones { get; set; }
    }
}
