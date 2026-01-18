using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Ciclos
{
    public class CreateCicloEscolarDto
    {
        [Required, MaxLength(20)]
        public string Clave { get; set; } = "";

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool EsActual { get; set; } = false;
    }
}
