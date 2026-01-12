using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdatePadreDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        // Usuario
        [Required] public string? Username { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? Nombre { get; set; }
        [Required] public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }

        // --- AGREGA ESTO ---
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }

        // Padre
        public string? Ocupacion { get; set; }
        public string? LugarTrabajo { get; set; }
        public string? TelefonoTrabajo { get; set; }
        public string? EstadoCivil { get; set; }
        public bool AceptaSMS { get; set; }
        public bool AceptaEmail { get; set; }

        // --- AGREGA ESTOS CAMPOS FALTANTES ---
        public string? Puesto { get; set; }
        public string? Carrera { get; set; }
        public string? NivelEstudios { get; set; }
        public string? DireccionTrabajo { get; set; }
    }
}
