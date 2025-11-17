using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{
    /// <summary>
    /// DTO para representar la información detallada de una asistencia al ser leída.
    /// </summary>
    public class AsistenciaDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreCompletoAlumno { get; set; }
        public int GrupoId { get; set; }
        public string NombreGrupo { get; set; }

        /// <summary>
        /// Fecha a la que corresponde la asistencia.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Estatus de la asistencia (ej: "Presente", "Falta", "Retardo", "Justificada").
        /// </summary>
        public string Estatus { get; set; }

        /// <summary>
        /// Hora de entrada del alumno (formato HH:mm).
        /// </summary>
        public string HoraEntrada { get; set; }

        /// <summary>
        /// Hora de salida del alumno (formato HH:mm).
        /// </summary>
        public string HoraSalida { get; set; }

        /// <summary>
        /// Minutos de retardo (si aplica).
        /// </summary>
        public int? MinutosRetardo { get; set; }

        /// <summary>
        /// Indica si la falta o retardo fue justificado.
        /// </summary>
        public bool Justificado { get; set; }

        /// <summary>
        /// Motivo de la justificación, permiso o modificación.
        /// </summary>
        public string Motivo { get; set; }

        /// <summary>
        /// URL del documento de justificación.
        /// </summary>
        public string JustificanteUrl { get; set; }

        /// <summary>
        /// Observaciones adicionales sobre la asistencia.
        /// </summary>
        public string Observaciones { get; set; }
    }
}
