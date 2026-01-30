using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Interfaces;

namespace SchoolSystem.Domain.Entities.Evaluacion
{
    /// <summary>
    /// Entidad de auditoría para registrar todas las recalificaciones realizadas en captura masiva
    /// Proporciona trazabilidad completa de cambios en calificaciones
    /// </summary>
    public class CalificacionAuditLog : BaseEntity, IAuditableEntity, ISoftDeletable
    {
        #region Referencia a Escuela (Multi-tenant)

        /// <summary>
        /// ID de la escuela a la que pertenece este registro de auditoría
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Referencia a la escuela
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        #endregion

        #region Referencias a Entidades Académicas

        /// <summary>
        /// ID de la calificación que fue modificada
        /// </summary>
        public int CalificacionId { get; set; }

        /// <summary>
        /// Referencia a la calificación (no cascada en delete)
        /// </summary>
        public virtual Calificacion Calificacion { get; set; }

        /// <summary>
        /// ID del alumno cuya calificación fue modificada
        /// Desnormalizado para rapidez de consultas
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// ID del grupo académico
        /// Desnormalizado para rapidez de consultas
        /// </summary>
        public int GrupoId { get; set; }

        /// <summary>
        /// ID de la materia
        /// Desnormalizado para rapidez de consultas
        /// </summary>
        public int MateriaId { get; set; }

        /// <summary>
        /// ID del período de evaluación
        /// Desnormalizado para rapidez de consultas
        /// </summary>
        public int PeriodoId { get; set; }

        #endregion

        #region Cambios de Calificación

        /// <summary>
        /// Calificación anterior (antes del cambio)
        /// </summary>
        public decimal CalificacionAnterior { get; set; }

        /// <summary>
        /// Calificación nueva (después del cambio)
        /// </summary>
        public decimal CalificacionNueva { get; set; }

        /// <summary>
        /// Diferencia en la calificación (CalificacionNueva - CalificacionAnterior)
        /// Valor calculado para análisis rápido
        /// </summary>
        public decimal Diferencia => CalificacionNueva - CalificacionAnterior;

        #endregion

        #region Observaciones

        /// <summary>
        /// Observaciones anteriores asociadas a la calificación
        /// Nullable
        /// </summary>
        public string? ObservacionesAnteriores { get; set; }

        /// <summary>
        /// Observaciones nuevas asociadas a la calificación
        /// Nullable
        /// </summary>
        public string? ObservacionesNuevas { get; set; }

        #endregion

        #region Metadata de Cambio

        /// <summary>
        /// Motivo de la recalificación (requerido)
        /// Ej: "Error en captura", "Revisión docente", "Solicitud alumno"
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public string Motivo { get; set; }

        /// <summary>
        /// ID del usuario que realizó la recalificación
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public int RecalificadoPor { get; set; }

        /// <summary>
        /// Fecha y hora UTC en que se realizó la recalificación
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime RecalificadoAtUtc { get; set; }

        /// <summary>
        /// Origen del cambio (opcional)
        /// Ej: "CapturaMasiva", "AdminPanel", "Api", "Import"
        /// </summary>
        public string? Origen { get; set; }

        /// <summary>
        /// CorrelationId para vincular múltiples cambios relacionados
        /// Útil para auditoría de operaciones en lote
        /// Nullable
        /// </summary>
        public string? CorrelationId { get; set; }

        #endregion

        #region Auditoría (IAuditableEntity)

        /// <summary>
        /// Fecha de creación de este registro de auditoría
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Usuario que creó este registro
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Usuario que últimamente actualizó este registro
        /// </summary>
        public int? UpdatedBy { get; set; }

        #endregion

        #region Soft Delete (ISoftDeletable)

        /// <summary>
        /// Indica si este registro de auditoría fue eliminado lógicamente
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Fecha en que fue eliminado lógicamente
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID del usuario que eliminó este registro
        /// </summary>
        public int? DeletedBy { get; set; }

        #endregion
    }
}
