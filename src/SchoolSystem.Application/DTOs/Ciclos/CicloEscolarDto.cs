using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Ciclos
{
    public class CicloEscolarDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = "";
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool EsActual { get; set; }
        public bool EstaCerrado { get; set; }
        public bool IsDeleted { get; set; }
    }
}
