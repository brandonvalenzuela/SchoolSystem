using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad GradoMateria - Relación entre Grado y Materia
    /// Define qué materias se imparten en cada grado
    /// </summary>
    public class GradoMateria : BaseEntity
    {
        /// <summary>
        /// ID de la escuela (Multi-tenant)
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// ID del grado
        /// </summary>
        public int GradoId { get; set; }

        /// <summary>
        /// Grado (Navigation Property)
        /// </summary>
        public virtual Grado Grado { get; set; }

        /// <summary>
        /// ID de la materia
        /// </summary>
        public int MateriaId { get; set; }

        /// <summary>
        /// Materia (Navigation Property)
        /// </summary>
        public virtual Materia Materia { get; set; }

        /// <summary>
        /// Horas semanales de la materia en este grado
        /// </summary>
        public int? HorasSemanales { get; set; }

        /// <summary>
        /// Orden de visualización de la materia
        /// </summary>
        public int? Orden { get; set; }

        /// <summary>
        /// Indica si la materia es obligatoria
        /// </summary>
        public bool Obligatoria { get; set; }

        /// <summary>
        /// Porcentaje de peso de la materia en el promedio general
        /// </summary>
        public decimal? PorcentajePeso { get; set; }

        public GradoMateria()
        {
            Obligatoria = true;
        }
    }
}
