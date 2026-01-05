using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateExpedienteDto
    {
        public int EscuelaId { get; set; } = 1;
        [Required] public int AlumnoId { get; set; }

        [Required] public string TipoSangre { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }

        [Required(ErrorMessage = "El contacto de emergencia es obligatorio")]
        public string ContactoEmergenciaNombre { get; set; }

        [Required(ErrorMessage = "El teléfono de emergencia es obligatorio")]
        public string ContactoEmergenciaTelefono { get; set; }

        public string ContactoEmergenciaParentesco { get; set; }
        public string Alergias { get; set; } // Texto simple
    }
}
