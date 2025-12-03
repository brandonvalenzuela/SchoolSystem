using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{
    public class DiaAsistenciaDto
    {
        public int Dia { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; } // "P", "F", "R", "J" (Letra corta para la tabla)
        public string EstatusCompleto { get; set; } // Para el tooltip
    }
}
