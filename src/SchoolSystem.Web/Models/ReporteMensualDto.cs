namespace SchoolSystem.Web.Models
{
    public class ReporteMensualDto
    {
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public string Matricula { get; set; }
        public List<DiaAsistenciaDto> Dias { get; set; }
        public int Asistencias { get; set; }
        public int Faltas { get; set; }
        public int Retardos { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
