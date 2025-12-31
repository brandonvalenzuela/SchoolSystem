using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateCalificacionMasivaDto
    {
        [Required]
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "Seleccione un grupo")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "Seleccione una materia")]
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "Seleccione un periodo")]
        public int PeriodoId { get; set; }

        // El ID del maestro que captura (el usuario logueado)
        public int CapturadoPor { get; set; }

        public List<CalificacionAlumnoDto> Calificaciones { get; set; } = new();
    }
}
