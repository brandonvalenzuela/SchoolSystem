using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CalificacionAlumnoDto
    {
        public int AlumnoId { get; set; }
        public string? Matricula { get; set; }
        public string? NombreAlumno { get; set; }

        public decimal CalificacionNumerica { get; set; }
        public string? Observaciones { get; set; }

        // UI flags de "punto 5"
        public bool YaTieneCalificacion { get; set; }
        public decimal? CalificacionActual { get; set; }
        public string? ObservacionesActuales { get; set; }

        // UI: cuando el usuario decide recalificar, habilitamos edición en estos
        public bool HabilitadoParaEdicion { get; set; }

        // PASO 14: Preview del estado (Insertar, Actualizar, OmitirExistente, Error, Pendiente)
        public string EstadoPreview { get; set; } = "Pendiente";

        /// <summary>
        /// Motivo del estado del preview (ej: "Alumno no está inscrito", "Período cerrado", etc.)
        /// </summary>
        public string? MotivoPreview { get; set; }

                /// <summary>
                /// AUDITORÍA: Motivo de recalificación. Se propaga cuando el usuario activa "Recalificar".
                /// Se usa para auditoría en la BD (tabla Calificacion.MotivoModificacion).
                /// </summary>
                public string? MotivoModificacion { get; set; }

                /// <summary>
                /// ✅ PASO 6B: Flag para marcar si hubo error al guardar
                /// Se usa para resaltar la fila en rojo en la tabla de errores
                /// </summary>
                public bool TieneError { get; set; }

                /// <summary>
                /// ✅ PASO 6B: Motivo del error si lo hubo al guardar
                /// </summary>
                public string? ErrorMotivo { get; set; }
            }
        }
