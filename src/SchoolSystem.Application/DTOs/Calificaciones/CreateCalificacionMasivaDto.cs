using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    public class CreateCalificacionMasivaDto
    {
        [Required] public int EscuelaId { get; set; }

        [Required] public int GrupoId { get; set; }

        [Required] public int MateriaId { get; set; }

        [Required] public int PeriodoId { get; set; }

        [Required] public int CapturadoPor { get; set; }

        [Required, MinLength(1, ErrorMessage = "Debe haber al menos una calificación.")]
        public List<CalificacionAlumnoDto> Calificaciones { get; set; }

        public bool PermitirRecalificarExistentes { get; set; } = false;

        public bool SoloValidar { get; set; } = false;

        /// <summary>
        /// Motivo de la modificación/recalificación. 
        /// Requerido si PermitirRecalificarExistentes es true y hay calificaciones existentes para actualizar.
        /// Se usa para auditoría: MotivoModificacion en la entidad Calificacion.
        /// </summary>
        public string? MotivoModificacion { get; set; }

        /// <summary>
        /// Alias para MotivoModificacion. Si se proporciona MotivoRecalificacion, se usa en lugar de MotivoModificacion.
        /// </summary>
        public string? MotivoRecalificacion 
        { 
            get => MotivoModificacion;
            set => MotivoModificacion = value;
        }
    }
}
