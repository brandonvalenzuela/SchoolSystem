using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Clase base abstracta para todas las entidades con una clave primaria numérica.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Identificador

        /// <summary>
        /// Identificador único de la entidad
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #endregion
    }
}

