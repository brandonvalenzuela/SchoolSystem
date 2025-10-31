using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Asistencia
{

    /// <summary>
    /// Estado de asistencia del alumno
    /// </summary>
    public enum EstadoAsistencia
    {
        /// <summary>
        /// Alumno presente
        /// </summary>
        Presente = 1,

        /// <summary>
        /// Alumno ausente sin justificación
        /// </summary>
        Falta = 2,

        /// <summary>
        /// Alumno llegó tarde
        /// </summary>
        Retardo = 3,

        /// <summary>
        /// Falta justificada
        /// </summary>
        Justificada = 4,

        /// <summary>
        /// Permiso autorizado
        /// </summary>
        Permiso = 5
    }
}
