using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Inscripciones
{
    /// <summary>
    /// DTO para representar la información detallada de una inscripción al ser leída.
    /// </summary>
    public class InscripcionDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreCompletoAlumno { get; set; }
        public int GrupoId { get; set; }
        public string NombreCompletoGrupo { get; set; }
        public int CicloEscolarId { get; set; }
        public string CicloEscolarClave { get; set; } = "";

        public DateTime FechaInscripcion { get; set; }

        /// <summary>
        /// Estatus de la inscripción (ej: "Inscrito", "Baja Temporal", "Finalizado").
        /// </summary>
        public string Estatus { get; set; }

        public int? NumeroLista { get; set; }
        public decimal? PromedioAcumulado { get; set; }
        public decimal? PorcentajeAsistencia { get; set; }
        public bool Becado { get; set; }
        public string? TipoBeca { get; set; }
        public decimal? PorcentajeBeca { get; set; }
        public bool Repetidor { get; set; }
        public string Matricula { get; set; }
    }
}
