using System;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Interfaz para entidades que requieren campos de auditoría de creación y modificación.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Fecha y hora de creación del registro.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ID del usuario que creó el registro.
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que realizó la última actualización.
        /// </summary>
        public int? UpdatedBy { get; set; }
    }
}