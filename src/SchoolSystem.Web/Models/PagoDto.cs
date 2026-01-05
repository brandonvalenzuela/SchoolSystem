namespace SchoolSystem.Web.Models
{
    public class PagoDto
    {
        public int Id { get; set; }
        public string DescripcionCargo { get; set; }
        public string NombreAlumno { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } // Efectivo, Transferencia
        public DateTime FechaPago { get; set; }
        public string FolioRecibo { get; set; }
        public bool Cancelado { get; set; }
        public string Observaciones { get; set; }
    }
}
