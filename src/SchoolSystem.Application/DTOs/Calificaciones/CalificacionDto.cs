using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    /// <summary>
    /// DTO para representar la información detallada de una calificación al ser leída.
    /// </summary>
    public class CalificacionDto
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }
        public string NombreCompletoAlumno { get; set; }

        public int MateriaId { get; set; }
        public string NombreMateria { get; set; }

        public int GrupoId { get; set; }
        public string NombreGrupo { get; set; }

        public int PeriodoId { get; set; }
        public string NombrePeriodo { get; set; }

        /// <summary>
        /// Calificación numérica obtenida.
        /// </summary>
        public decimal CalificacionNumerica { get; set; }

        /// <summary>
        /// Calificación en formato de letra (ej: "A", "B").
        /// </summary>
        public string CalificacionLetra { get; set; }

        /// <summary>
        /// Indica si el alumno aprobó.
        /// </summary>
        public bool Aprobado { get; set; }

        /// <summary>
        /// Tipo de evaluación (ej: "Examen Final", "Promedio Tareas").
        /// </summary>
        public string TipoEvaluacion { get; set; }

        /// <summary>
        /// Peso de esta calificación en el total del período.
        /// </summary>
        public decimal? Peso { get; set; }

        /// <summary>
        /// Observaciones generales del maestro.
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Fortalezas identificadas en el desempeño del alumno.
        /// </summary>
        public string Fortalezas { get; set; }

        /// <summary>
        /// Áreas de oportunidad para el alumno.
        /// </summary>
        public string AreasOportunidad { get; set; }

        /// <summary>
        /// Recomendaciones para mejorar.
        /// </summary>
        public string Recomendaciones { get; set; }

        /// <summary>
        /// Fecha en que se capturó o modificó la calificación.
        /// </summary>
        public DateTime FechaCaptura { get; set; }

        /// <summary>
        /// Nombre del maestro que capturó la calificación.
        /// </summary>
        public string NombreMaestroCaptura { get; set; }
    }
}
