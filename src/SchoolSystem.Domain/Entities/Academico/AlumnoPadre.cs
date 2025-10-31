using SchoolSystem.Domain.Entities.Common;
using SchoolSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Entities.Academico
{
    /// <summary>
    /// Entidad AlumnoPadre - Relación muchos a muchos entre Alumnos y Padres
    /// </summary>
    public class AlumnoPadre : BaseEntity
    {
        /// <summary>
        /// ID del alumno
        /// </summary>
        public int AlumnoId { get; set; }

        /// <summary>
        /// Alumno (Navigation Property)
        /// </summary>
        public virtual Alumno Alumno { get; set; }

        /// <summary>
        /// ID del padre/tutor
        /// </summary>
        public int PadreId { get; set; }

        /// <summary>
        /// Padre/tutor (Navigation Property)
        /// </summary>
        public virtual Padre Padre { get; set; }

        /// <summary>
        /// Relación familiar con el alumno
        /// </summary>
        public RelacionFamiliar Relacion { get; set; }

        /// <summary>
        /// Indica si es el tutor principal del alumno
        /// </summary>
        public bool EsTutorPrincipal { get; set; }

        /// <summary>
        /// Indica si está autorizado para recoger al alumno
        /// </summary>
        public bool AutorizadoRecoger { get; set; }

        /// <summary>
        /// Indica si debe recibir notificaciones sobre el alumno
        /// </summary>
        public bool RecibeNotificaciones { get; set; }

        /// <summary>
        /// Indica si vive con el alumno
        /// </summary>
        public bool ViveConAlumno { get; set; }
    }
}
