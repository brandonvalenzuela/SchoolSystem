using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Domain.Enums.Academico
{
    /// <summary>
    /// Áreas académicas o departamentos a los que puede pertenecer una materia
    /// </summary>
    public enum AreaAcademica
    {
        /// <summary>
        /// Ciencias exactas (Matemáticas, Física, Química)
        /// </summary>
        Ciencias = 0,

        /// <summary>
        /// Humanidades y Ciencias Sociales (Historia, Filosofía, Sociología)
        /// </summary>
        Humanidades = 1,

        /// <summary>
        /// Lenguas y Literatura (Español, Inglés, Francés)
        /// </summary>
        Lenguajes = 2,

        /// <summary>
        /// Artes y Cultura (Música, Pintura, Teatro, Danza)
        /// </summary>
        Artes = 3,

        /// <summary>
        /// Educación Física y Deportes
        /// </summary>
        Deportes = 4,

        /// <summary>
        /// Tecnología e Informática (Computación, Robótica)
        /// </summary>
        Tecnologia = 5,

        /// <summary>
        /// Ciencias Naturales (Biología, Ecología, Ciencias de la Tierra)
        /// </summary>
        CienciasNaturales = 6,

        /// <summary>
        /// Formación Cívica y Ética
        /// </summary>
        FormacionCivica = 7,

        /// <summary>
        /// Actividades extracurriculares y talleres
        /// </summary>
        Extracurricular = 8,

        /// <summary>
        /// Desarrollo personal y orientación vocacional
        /// </summary>
        DesarrolloPersonal = 9
    }
}
