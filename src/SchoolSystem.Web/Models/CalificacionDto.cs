namespace SchoolSystem.Web.Models
{
    public class CalificacionDto
    {
        public int Id { get; set; }
        public string NombreCompletoAlumno { get; set; }
        public string NombreMateria { get; set; }
        public string NombreGrupo { get; set; }
        public string NombrePeriodo { get; set; }
        public decimal CalificacionNumerica { get; set; }
        public string CalificacionLetra { get; set; }
        public bool Aprobado { get; set; }
        public string TipoEvaluacion { get; set; }
        public DateTime FechaCaptura { get; set; }
    }
}
