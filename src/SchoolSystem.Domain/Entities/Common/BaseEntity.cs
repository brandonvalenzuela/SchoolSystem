using System;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Entidad base para la mayoría de las entidades del dominio.
    /// Proporciona identificador único.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Identificador

        /// <summary>
        /// Identificador único de la entidad
        /// </summary>
        public int Id { get; set; }

        #endregion
    }
}

