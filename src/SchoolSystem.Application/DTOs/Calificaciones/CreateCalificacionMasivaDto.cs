using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    public class CreateCalificacionMasivaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required]
        public int GrupoId { get; set; }

        [Required]
        public int MateriaId { get; set; }

        [Required]
        public int PeriodoId { get; set; }

        [Required]
        public int CapturadoPor { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe haber al menos una calificación.")]
        public List<CalificacionAlumnoDto> Calificaciones { get; set; }
    }
}