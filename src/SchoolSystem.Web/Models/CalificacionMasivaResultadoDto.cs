namespace SchoolSystem.Web.Models
{
    public class CalificacionMasivaResultadoDto
    {
        public int Insertadas { get; set; }
        public int Actualizadas { get; set; }

        public List<int> AlumnoIdsInsertados { get; set; } = new();
        public List<int> AlumnoIdsActualizados { get; set; } = new();

        public List<CalificacionMasivaExistenteDto> Existentes { get; set; } = new();
        public List<CalificacionMasivaErrorDto> Errores { get; set; } = new();
        public bool PermiteRecalificar { get; set; }
        public string? MotivoNoPermiteRecalificar { get; set; }
        public int TotalEnviadas { get; set; }
    }
}
