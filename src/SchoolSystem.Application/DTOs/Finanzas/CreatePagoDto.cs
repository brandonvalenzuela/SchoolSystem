using SchoolSystem.Domain.Enums.Finanzas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Finanzas
{
    public class CreatePagoDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required]
        public int CargoId { get; set; }

        [Required]
        public int AlumnoId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public string Referencia { get; set; } // Para transferencia/cheque
        public string Banco { get; set; }
        public string Observaciones { get; set; }

        [Required]
        public int RecibidoPorId { get; set; } // Usuario que cobra
    }
}
