using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateGradoDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "El nivel educativo es obligatorio")]
        public int NivelEducativoId { get; set; } // 1=Kinder, 2=Primaria...

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required]
        public int Orden { get; set; }

        public bool Activo { get; set; } = true;

        // Opcionales
        public int? EdadRecomendada { get; set; }
        public int? CapacidadMaximaPorGrupo { get; set; }
    }
}
