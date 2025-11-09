using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Tipos de materia según su modalidad de enseñanza
    /// </summary>
    public enum TipoMateria
    {
        /// <summary>
        /// Materia teórica basada en conceptos y conocimientos
        /// </summary>
        Teorica = 0,

        /// <summary>
        /// Materia práctica con ejercicios y aplicaciones
        /// </summary>
        Practica = 1,

        /// <summary>
        /// Taller con actividades manuales o técnicas
        /// </summary>
        Taller = 2,

        /// <summary>
        /// Materia de laboratorio con experimentos
        /// </summary>
        Laboratorio = 3,

        /// <summary>
        /// Materia teórico-práctica (combinación)
        /// </summary>
        TeoricoPractica = 4,

        /// <summary>
        /// Seminario o curso especializado
        /// </summary>
        Seminario = 5,

        /// <summary>
        /// Curso en línea o virtual
        /// </summary>
        Virtual = 6
    }
}
