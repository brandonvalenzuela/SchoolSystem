using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums
{
    /// <summary>
    /// Relación familiar entre alumno y tutor
    /// </summary>
    public enum RelacionFamiliar
    {
        /// <summary>
        /// Padre
        /// </summary>
        Padre = 1,

        /// <summary>
        /// Madre
        /// </summary>
        Madre = 2,

        /// <summary>
        /// Tutor legal (sin parentesco)
        /// </summary>
        Tutor = 3,

        /// <summary>
        /// Abuelo
        /// </summary>
        Abuelo = 4,

        /// <summary>
        /// Abuela
        /// </summary>
        Abuela = 5,

        /// <summary>
        /// Tío
        /// </summary>
        Tio = 6,

        /// <summary>
        /// Tía
        /// </summary>
        Tia = 7,

        /// <summary>
        /// Hermano
        /// </summary>
        Hermano = 8,

        /// <summary>
        /// Hermana
        /// </summary>
        Hermana = 9,

        /// <summary>
        /// Otro familiar
        /// </summary>
        Otro = 10
    }
}
