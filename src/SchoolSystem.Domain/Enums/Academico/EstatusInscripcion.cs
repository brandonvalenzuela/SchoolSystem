using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Estado de inscripción del alumno
    /// </summary>
    public enum EstatusInscripcion
    {
        /// <summary>
        /// Inscrito y cursando
        /// </summary>
        Inscrito = 1,

        /// <summary>
        /// Baja temporal
        /// </summary>
        BajaTemporal = 2,

        /// <summary>
        /// Baja definitiva del ciclo
        /// </summary>
        BajaDefinitiva = 3,

        /// <summary>
        /// Ciclo finalizado
        /// </summary>
        Finalizado = 4
    }
}
