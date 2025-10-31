using System;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Interfaz para entidades que requieren auditoría
    /// Proporciona propiedades para rastrear cuándo y quién creó/modificó una entidad
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Fecha y hora de creación de la entidad
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID del usuario que creó la entidad (opcional)
        /// </summary>
        int? CreatedBy { get; set; }

        /// <summary>
        /// ID del usuario que realizó la última actualización (opcional)
        /// </summary>
        int? UpdatedBy { get; set; }
    }
}