using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificaciones
{
    public class CalificacionMasivaResultadoDto
    {
        public int Insertadas { get; set; }
        public int Actualizadas { get; set; }

        public List<int> AlumnoIdsInsertados { get; set; } = new();
        public List<int> AlumnoIdsActualizados { get; set; } = new();

        public List<CalificacionMasivaExistenteDto> Existentes { get; set; } = new();
        public List<CalificacionMasivaErrorDto> Errores { get; set; } = new();
        public bool PermiteRecalificar { get; set; }
        public string? MotivoNoPermiteRecalificar { get; set; }
        public int TotalEnviadas { get; set; }

        // PASO 14: Preview detallado por alumno
        /// <summary>
        /// Preview por alumno indicando qué pasaría si se ejecutara el guardado.
        /// Se llena cuando SoloValidar=true.
        /// </summary>
        public List<CalificacionMasivaPreviewAlumnoDto> PreviewPorAlumno { get; set; } = new();

        /// <summary>
        /// Conteos agregados basados en PreviewPorAlumno
        /// </summary>
        public int TotalInsertarias { get; set; }
        public int TotalActualizarias { get; set; }
        public int TotalOmitirias { get; set; }
        public int TotalErrores { get; set; }

        // Auditoría: Indica si se requiere motivo de recalificación
        /// <summary>
        /// Cuando SoloValidar=true y existen calificaciones para recalificar:
        /// true si se requiere MotivoRecalificacion en el próximo commit.
        /// </summary>
        public bool RequiereMotivoRecalificacion { get; set; }

        // Propiedades para manejo de errores de concurrencia
        public bool Exitoso { get; set; } = true; // Por defecto, exitoso
        public string? Mensaje { get; set; } // Mensaje de error o información
    }
}
