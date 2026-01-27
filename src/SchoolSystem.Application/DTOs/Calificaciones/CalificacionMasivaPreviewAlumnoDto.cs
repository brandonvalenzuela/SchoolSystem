namespace SchoolSystem.Application.DTOs.Calificaciones
{
    /// <summary>
    /// DTO para el preview por alumno en captura masiva de calificaciones.
    /// Indica qué pasaría si se ejecutara el guardado.
    /// </summary>
    public class CalificacionMasivaPreviewAlumnoDto
    {
        /// <summary>
        /// ID del alumno
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Estado de la operación que se realizaría:
        /// - Insertar: se crearía nueva calificación
        /// - Actualizar: se recalificaría la existente
        /// - OmitirExistente: existe pero no se permite recalificar
        /// - Error: no se puede procesar por alguna razón
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Motivo del estado (ej. "Ya existe calificación", "Período cerrado", etc.)
        /// </summary>
        public string? Motivo { get; set; }

        /// <summary>
        /// Calificación actual del alumno (si existe)
        /// </summary>
        public decimal? CalificacionActual { get; set; }

        /// <summary>
        /// Observaciones actuales del alumno (si existe)
        /// </summary>
        public string? ObservacionesActuales { get; set; }
    }
}
