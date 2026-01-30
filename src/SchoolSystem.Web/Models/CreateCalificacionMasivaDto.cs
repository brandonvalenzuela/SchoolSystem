using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateCalificacionMasivaDto
    {
        [Required]
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "Seleccione un grupo")]
        public int GrupoId { get; set; }

        [Required(ErrorMessage = "Seleccione una materia")]
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "Seleccione un periodo")]
        public int PeriodoId { get; set; }

        // El ID del maestro que captura (el usuario logueado)
        public int CapturadoPor { get; set; }

        public List<CalificacionAlumnoDto> Calificaciones { get; set; } = new();
        public bool PermitirRecalificarExistentes { get; set; } = false;
        public bool SoloValidar { get; set; } = false;

                /// <summary>
                /// Motivo de recalificación. Requerido si PermitirRecalificarExistentes es true y hay existentes.
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
