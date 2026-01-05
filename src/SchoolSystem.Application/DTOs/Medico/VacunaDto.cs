using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Medico
{
    public class VacunaDto
    {
        public int Id { get; set; }
        public string NombreVacuna { get; set; }
        public DateTime FechaAplicacion { get; set; }
    }
}
