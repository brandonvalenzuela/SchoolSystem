namespace SchoolSystem.Web.Models
{
    public class CargoDto
    {
        public int Id { get; set; }
        public string Concepto { get; set; } // Nombre del concepto de pago
        public decimal MontoTotal { get; set; } // Monto ya con mora/descuento
        public decimal SaldoPendiente { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estatus { get; set; } // Pendiente, Pagado, Vencido
        public int DiasRetraso { get; set; }
        public bool Seleccionado { get; set; } // Auxiliar para el checkbox en la UI
    }
}
