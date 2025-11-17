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
    /// DTO para el registro masivo de asistencias de un grupo.
    /// </summary>
    public class CreateAsistenciaMasivaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El ID del grupo es obligatorio.")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del usuario que registra.")]
        public int RegistradoPor { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe haber al menos un registro de asistencia.")]
        public List<AsistenciaAlumnoDto> Asistencias { get; set; }
    }

    /// <summary>
    /// DTO que representa la asistencia de un único alumno dentro de un registro masivo.
    /// </summary>
    public class AsistenciaAlumnoDto
    {
        [Required]
        public int AlumnoId { get; set; }

        [Required]
        public EstadoAsistencia Estatus { get; set; }

        public TimeSpan? HoraEntrada { get; set; }

        [Range(0, int.MaxValue)]
        public int? MinutosRetardo { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }
    }
}
