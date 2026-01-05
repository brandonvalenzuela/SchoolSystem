using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreatePagoDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required]
        public int AlumnoId { get; set; }

        [Required]
        public int CargoId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required]
        public int MetodoPago { get; set; } = 1; // 1=Efectivo

        public string? Referencia { get; set; } // Para transferencia
        public string? Observaciones { get; set; }
        public string? Banco { get; set; }

        public int RecibidoPorId { get; set; } // Se llenará con el usuario logueado
    }
}
