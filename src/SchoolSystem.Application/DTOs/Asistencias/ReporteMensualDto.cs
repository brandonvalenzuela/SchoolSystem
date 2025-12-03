using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{
    public class ReporteMensualDto
    {
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public string Matricula { get; set; }

        // Estadísticas del mes
        public int Faltas { get; set; }
        public int Retardos { get; set; }
        public int Asistencias { get; set; }
        public decimal Porcentaje { get; set; }

        // Lista de días (1 al 30/31)
        public List<DiaAsistenciaDto> Dias { get; set; }
    }
}
