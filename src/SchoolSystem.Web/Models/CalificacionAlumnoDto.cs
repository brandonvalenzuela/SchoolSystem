using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CalificacionAlumnoDto
    {
        public int AlumnoId { get; set; }
        public string? Matricula { get; set; }
        public string? NombreAlumno { get; set; }

        public decimal CalificacionNumerica { get; set; }
        public string? Observaciones { get; set; }

        // UI flags de “punto 5”
        public bool YaTieneCalificacion { get; set; }
        public decimal? CalificacionActual { get; set; }
        public string? ObservacionesActuales { get; set; }

        // UI: cuando el usuario decide recalificar, habilitamos edición en estos
        public bool HabilitadoParaEdicion { get; set; }
    }
}
