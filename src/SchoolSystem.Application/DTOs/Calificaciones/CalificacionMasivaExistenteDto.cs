using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    public class CalificacionMasivaExistenteDto
    {
        public int AlumnoId { get; set; }
        public decimal CalificacionActual { get; set; }
        public string? ObservacionesActuales { get; set; }
    }
}
