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

        // Propiedades para manejo de errores de concurrencia
        public bool Exitoso { get; set; } = true; // Por defecto, exitoso
        public string? Mensaje { get; set; } // Mensaje de error o información
    }
}
