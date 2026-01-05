using SchoolSystem.Web.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class AsistenciaAlumnoDto
    {
        public int AlumnoId { get; set; }

        public EstadoAsistencia Estatus { get; set; }

        public string? Observaciones { get; set; }
    }
}
