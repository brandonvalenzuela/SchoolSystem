using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateUsuarioDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string Telefono { get; set; }

        // 1=SuperAdmin, 2=Director, 3=Subdirector, 4=Administrativo
        // (Maestros, Padres y Alumnos se crean en sus propios módulos)
        [Required]
        public int Rol { get; set; }
    }
}
