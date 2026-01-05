using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Medico
{
    public class ExpedienteMedicoDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }

        // Básicos
        public string TipoSangre { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }
        public string Alergias { get; set; } // Texto resumen

        // Emergencia
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public string ContactoEmergenciaParentesco { get; set; }

        // Listas detalladas
        public List<AlergiaDto> AlergiasDetalladas { get; set; }
        public List<VacunaDto> Vacunas { get; set; }
    }
}
