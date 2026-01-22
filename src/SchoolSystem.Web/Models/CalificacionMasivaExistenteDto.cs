namespace SchoolSystem.Web.Models
{
    public class CalificacionMasivaExistenteDto
    {
        public int AlumnoId { get; set; }
        public decimal CalificacionActual { get; set; }
        public string? ObservacionesActuales { get; set; }
    }
}
