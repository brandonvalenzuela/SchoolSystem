using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CancelarPagoDto
    {
        [Required(ErrorMessage = "El motivo es obligatorio")]
        public string Motivo { get; set; }
    }
}
