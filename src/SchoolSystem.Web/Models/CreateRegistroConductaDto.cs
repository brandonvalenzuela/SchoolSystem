using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateRegistroConductaDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "Seleccione un alumno")]
        [Range(1, int.MaxValue, ErrorMessage = "Alumno inválido")]
        public int AlumnoId { get; set; }

        public int MaestroId { get; set; } // Se llenará con el usuario logueado

        [Required]
        public int Tipo { get; set; } = 1; // 1=Positiva, 2=Negativa (Enum)

        [Required]
        public int Categoria { get; set; } = 1; // Enum

        public int? Gravedad { get; set; } // Enum (Solo para negativas)

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int Puntos { get; set; } = 0;

        public DateTime FechaHoraIncidente { get; set; } = DateTime.Now;
    }
}
