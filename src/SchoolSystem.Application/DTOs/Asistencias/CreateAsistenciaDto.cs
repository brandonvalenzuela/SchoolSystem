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
    /// DTO para crear un único registro de asistencia.
    /// </summary>
    public class CreateAsistenciaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [Required(ErrorMessage = "El ID del alumno es obligatorio.")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El ID del grupo es obligatorio.")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El estatus es obligatorio.")]
        public EstadoAsistencia Estatus { get; set; }

        public TimeSpan? HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Los minutos de retardo no pueden ser negativos.")]
        public int? MinutosRetardo { get; set; }

        [StringLength(1000)]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Se requiere el ID del usuario que registra.")]
        public int RegistradoPor { get; set; }
    }
}
