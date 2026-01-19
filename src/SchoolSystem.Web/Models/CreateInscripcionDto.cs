using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateInscripcionDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "Debes seleccionar un alumno")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un alumno válido")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un grupo")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un grupo válido")]
        public int GrupoId { get; set; }
        public int? NumeroLista { get; set; }
        public bool Becado { get; set; }
        public bool Repetidor { get; set; }
        public string? Observaciones { get; set; }
        public string? TipoBeca { get; set; }
        public decimal? PorcentajeBeca { get; set; }
    }
}
