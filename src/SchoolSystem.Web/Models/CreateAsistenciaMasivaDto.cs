using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateAsistenciaMasivaDto
    {
        public int EscuelaId { get; set; }

        [Required]
        public int GrupoId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public int RegistradoPor { get; set; }

        public List<AsistenciaAlumnoDto> Asistencias { get; set; }
    }
}
