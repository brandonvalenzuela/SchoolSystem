using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Medico
{
    public class UpdateExpedienteDto
    {
        public int Id { get; set; }
        public string TipoSangre { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }
        public string Alergias { get; set; }
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public string ContactoEmergenciaParentesco { get; set; }
    }
}
