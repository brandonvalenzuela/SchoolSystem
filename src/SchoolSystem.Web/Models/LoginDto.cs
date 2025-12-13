using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
