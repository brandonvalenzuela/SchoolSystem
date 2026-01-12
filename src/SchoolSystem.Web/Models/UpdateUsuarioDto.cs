using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }

        [Required] public string? Username { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? Nombre { get; set; }
        [Required] public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public int Rol { get; set; }
    }
}
