using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Escuelas
{
    /// <summary>
    /// Tipos de planes de suscripción disponibles para las escuelas
    /// </summary>
    public enum TipoPlan
    {
        /// <summary>
        /// Plan gratuito de prueba con funcionalidades limitadas
        /// </summary>
        Prueba = 0,

        /// <summary>
        /// Plan básico para escuelas pequeñas
        /// Hasta 100 alumnos y funcionalidades esenciales
        /// </summary>
        Basico = 1,

        /// <summary>
        /// Plan estándar para escuelas medianas
        /// Hasta 300 alumnos con funcionalidades avanzadas
        /// </summary>
        Estandar = 2,

        /// <summary>
        /// Plan profesional para escuelas grandes
        /// Hasta 500 alumnos con todas las funcionalidades
        /// </summary>
        Profesional = 3,

        /// <summary>
        /// Plan premium para escuelas muy grandes
        /// Hasta 1000 alumnos con funcionalidades premium
        /// </summary>
        Premium = 4,

        /// <summary>
        /// Plan enterprise para instituciones educativas
        /// Sin límite de alumnos, soporte prioritario y personalización
        /// </summary>
        Enterprise = 5
    }
}
