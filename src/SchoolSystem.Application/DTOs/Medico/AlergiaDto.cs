using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Medico
{
    public class AlergiaDto
    {
        public int Id { get; set; }
        public string NombreAlergeno { get; set; }
        public string Sintomas { get; set; }
        public string TratamientoRecomendado { get; set; }
        public bool EsGrave { get; set; }
    }
}
