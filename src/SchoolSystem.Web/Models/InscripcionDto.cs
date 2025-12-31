namespace SchoolSystem.Web.Models
{
    public class InscripcionDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreCompletoAlumno { get; set; }
        public int GrupoId { get; set; }
        public string NombreCompletoGrupo { get; set; } // "1° de Primaria - Grupo A"
        public string? CicloEscolar { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estatus { get; set; } // Inscrito, Baja, Finalizado
        public int? NumeroLista { get; set; }
        public decimal? PromedioAcumulado { get; set; }
        public string Matricula { get; set; }
    }
}
