using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{
    public class JustificarFaltaDto
    {
        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(500)]
        public string Motivo { get; set; }

        [StringLength(500)]
        public string JustificanteUrl { get; set; } // URL del archivo (PDF/Img)
    }
}
