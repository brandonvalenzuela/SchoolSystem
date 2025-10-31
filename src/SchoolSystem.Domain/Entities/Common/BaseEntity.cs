using System;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Entidad base para la mayoría de las entidades del dominio.
    /// Proporciona identificador único y propiedades comunes de auditoría y borrado lógico.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Identificador

        /// <summary>
        /// Identificador único de la entidad (GUID)
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region Propiedades de Auditoría

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Usuario que creó el registro (opcional, cadena para flexibilidad)
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Usuario que realizó la última actualización (opcional, cadena para flexibilidad)
        /// </summary>
        public string UpdatedBy { get; set; }

        #endregion

        #region Soft Delete (Borrado Lógico)

        /// <summary>
        /// Indica si la entidad ha sido eliminada lógicamente
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion
    }
}

