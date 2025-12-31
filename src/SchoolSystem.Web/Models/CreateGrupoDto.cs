using SchoolSystem.Web.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateGrupoDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El grado es obligatorio.")]
        public int GradoId { get; set; }

        [Required(ErrorMessage = "El nombre del grupo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El ciclo escolar es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ciclo escolar no puede exceder los 20 caracteres.")]
        public string CicloEscolar { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "La capacidad máxima debe estar entre 1 y 100.")]
        public int CapacidadMaxima { get; set; }

        public int? MaestroTitularId { get; set; }

        [StringLength(100)]
        public string Aula { get; set; }

        public Turno? Turno { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public TimeSpan? HoraFin { get; set; }

        [StringLength(100)]
        public string DiasClase { get; set; }

        public bool Activo { get; set; } = true;
    }
}
