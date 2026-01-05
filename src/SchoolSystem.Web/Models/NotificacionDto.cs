namespace SchoolSystem.Web.Models
{
    public class NotificacionDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; } // Academico, Financiero
        public string Prioridad { get; set; } // Alta, Normal
        public bool Leida { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string UrlAccion { get; set; }
        public string Icono { get; set; }
        public string Color { get; set; }
    }
}
