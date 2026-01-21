using SchoolSystem.Domain.Enums.Asistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Asistencias
{

    /// <summary>
    /// DTO que representa la asistencia de un único alumno dentro de un registro masivo.
    /// </summary>
    public class AsistenciaAlumnoDto
    {
        [Required] public int AlumnoId { get; set; }
        [Required] public EstadoAsistencia Estatus { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        [Range(0, int.MaxValue)] public int? MinutosRetardo { get; set; }
        [StringLength(500)] public string? Observaciones { get; set; }
    }
}
