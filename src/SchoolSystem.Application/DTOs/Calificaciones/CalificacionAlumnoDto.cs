using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    /// <summary>
    /// Representa la calificación de un único alumno en una captura masiva.
    /// </summary>
    public class CalificacionAlumnoDto
    {
        [Required]
        public int AlumnoId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal CalificacionNumerica { get; set; }

        public string? Observaciones { get; set; }
    }
}
