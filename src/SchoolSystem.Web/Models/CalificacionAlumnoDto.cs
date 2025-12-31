using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CalificacionAlumnoDto
    {
        public int AlumnoId { get; set; }

        // Propiedades auxiliares para la vista (no se envían al backend necesariamente, pero ayudan)
        public string NombreAlumno { get; set; }
        public string Matricula { get; set; }

        [Range(0, 10, ErrorMessage = "La nota debe estar entre 0 y 10")]
        public decimal CalificacionNumerica { get; set; }

        public string Observaciones { get; set; }
    }
}
