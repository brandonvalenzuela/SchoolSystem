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

        // PASO 14: Preview detallado por alumno
        /// <summary>
        /// Preview por alumno indicando qué pasaría si se ejecutara el guardado.
        /// Se llena cuando SoloValidar=true.
        /// </summary>
        public List<CalificacionMasivaPreviewAlumnoDto> PreviewPorAlumno { get; set; } = new();

        /// <summary>
        /// Conteos agregados basados en PreviewPorAlumno
        /// </summary>
        public int TotalInsertarias { get; set; }
        public int TotalActualizarias { get; set; }
        public int TotalOmitirias { get; set; }
        public int TotalErrores { get; set; }

        // Propiedades para manejo de errores de concurrencia
        public bool Exitoso { get; set; } = true; // Por defecto, exitoso
        public string? Mensaje { get; set; } // Mensaje de error o información
    }

    public class CalificacionMasivaPreviewAlumnoDto
    {
        public int AlumnoId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
        public decimal? CalificacionActual { get; set; }
        public string? ObservacionesActuales { get; set; }
    }
}

