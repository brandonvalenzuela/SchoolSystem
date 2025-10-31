using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Estado del alumno en el sistema
    /// </summary>
    public enum EstatusAlumno
    {
        /// <summary>
        /// Alumno activo y cursando
        /// </summary>
        Activo = 1,

        /// <summary>
        /// Alumno inactivo temporalmente
        /// </summary>
        Inactivo = 2,

        /// <summary>
        /// Alumno que completó sus estudios
        /// </summary>
        Egresado = 3,

        /// <summary>
        /// Alumno dado de baja definitivamente
        /// </summary>
        Baja = 4
    }
}
