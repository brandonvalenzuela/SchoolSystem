namespace SchoolSystem.Web.Models
{
    public class ExpedienteMedicoDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public string TipoSangre { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }
        public string Alergias { get; set; }
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public string ContactoEmergenciaParentesco { get; set; }

        // Listas (puedes dejarlas vacías por ahora si no las usas en la vista simple)
        public List<AlergiaDto> AlergiasDetalladas { get; set; } = new();
        public List<VacunaDto> Vacunas { get; set; } = new();
    }
}
