using System;

namespace SchoolSystem.Domain.Entities.Common
{
    /// <summary>
    /// Interfaz para entidades que soportan borrado lógico (soft delete)
    /// Permite "eliminar" registros sin borrarlos físicamente de la base de datos
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Indica si la entidad ha sido eliminada lógicamente
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Fecha y hora en que se eliminó la entidad
        /// </summary>
        DateTime? DeletedAt { get; set; }

        /// <summary>
        /// ID del usuario que eliminó la entidad
        /// </summary>
        int? DeletedBy { get; set; }
    }
}