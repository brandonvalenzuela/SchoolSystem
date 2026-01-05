using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Finanzas
{
    public class CancelarPagoDto
    {
        [Required(ErrorMessage = "El motivo es obligatorio")]
        public string Motivo { get; set; }
    }
}
