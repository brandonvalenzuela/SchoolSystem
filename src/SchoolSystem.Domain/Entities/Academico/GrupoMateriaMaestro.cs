using SchoolSystem.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad GrupoMateriaMaestro - Asignación de un maestro a una materia en un grupo
    /// </summary>
    public class GrupoMateriaMaestro : BaseEntity
    {
        /// <summary>
        /// ID de la escuela (Multi-tenant)
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// ID del grupo
        /// </summary>
        public int GrupoId { get; set; }

        /// <summary>
        /// Grupo (Navigation Property)
        /// </summary>
        public virtual Grupo Grupo { get; set; }

        /// <summary>
        /// ID de la materia
        /// </summary>
        public int MateriaId { get; set; }

        /// <summary>
        /// Materia (Navigation Property)
        /// </summary>
        public virtual Materia Materia { get; set; }

        /// <summary>
        /// ID del maestro
        /// </summary>
        public int MaestroId { get; set; }

        /// <summary>
        /// Maestro (Navigation Property)
        /// </summary>
        public virtual Maestro Maestro { get; set; }

        /// <summary>
        /// Ciclo escolar de la asignación
        /// </summary>
        public string CicloEscolar { get; set; }
        public int? CicloEscolarId { get; set; }
        public virtual CicloEscolar? Ciclo { get; set; }  // nombre corto para evitar choque

        /// <summary>
        /// Horario de la materia (opcional)
        /// </summary>
        public string Horario { get; set; }
    }
}
