using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Entities.Conducta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Domain.Entities.Conducta
{
    /// <summary>
    /// Insignias o logros que pueden obtener los alumnos
    /// </summary>
    [Table("Insignias")]
    public class Insignia : BaseEntity, IAuditableEntity
    {
        /// <summary>
        /// Identificador de la escuela (multi-tenant)
        /// </summary>
        [Required]
        public int EscuelaId { get; set; }

        /// <summary>
        /// Nombre de la insignia
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        /// <summary>
        /// URL o nombre del archivo del icono
        /// </summary>
        [StringLength(500)]
        public string Icono { get; set; }

        /// <summary>
        /// Descripción de la insignia
        /// </summary>
        [Required]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Criterios para obtener la insignia
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Criterios { get; set; }

        /// <summary>
        /// Tipo de insignia (Académica, Conducta, Especial, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        /// <summary>
        /// Rareza de la insignia (Común, Rara, Épica, Legendaria)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Rareza { get; set; }

        /// <summary>
        /// Puntos que otorga al obtenerla
        /// </summary>
        [Required]
        public int PuntosOtorgados { get; set; }

        /// <summary>
        /// Requisitos en formato JSON o texto
        /// </summary>
        [StringLength(1000)]
        public string? Requisitos { get; set; }

        /// <summary>
        /// Si es recurrente se puede obtener múltiples veces
        /// </summary>
        [Required]
        public bool EsRecurrente { get; set; }

        /// <summary>
        /// Estado de la insignia
        /// </summary>
        [Required]
        public bool Activo { get; set; }

        #region Auditoría

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        #endregion

        #region Relaciones

        /// <summary>
        /// Alumnos que han ganado esta insignia
        /// </summary>
        public virtual ICollection<AlumnoInsignia> AlumnosQueGanaron { get; set; }

        #endregion

        #region Constructor

        public Insignia()
        {
            Activo = true;
            EsRecurrente = false;
            AlumnosQueGanaron = new HashSet<AlumnoInsignia>();
        }

        #endregion
    }
}
