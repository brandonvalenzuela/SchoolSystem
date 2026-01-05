using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Medico
{
    public class CreateExpedienteDto
    {
        [Required] public int EscuelaId { get; set; } = 1;
        [Required] public int AlumnoId { get; set; }
        public string TipoSangre { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }

        [Required] public string ContactoEmergenciaNombre { get; set; }
        [Required] public string ContactoEmergenciaTelefono { get; set; }
        [Required] public string ContactoEmergenciaParentesco { get; set; }
    }
}
