using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Finanzas
{
    public class CargoDto
    {
        public int Id { get; set; }
        public string Concepto { get; set; }
        public decimal MontoTotal { get; set; } // Monto + Mora - Descuento
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estatus { get; set; } // Pendiente, Vencido, Pagado
        public int DiasRetraso { get; set; }
    }
}
