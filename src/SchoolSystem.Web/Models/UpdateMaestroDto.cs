using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateMaestroDto
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

        // --- Datos Faltantes que causaban error ---
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }

        // Maestro
        [Required] public string? NumeroEmpleado { get; set; }
        [Required] public string? Especialidad { get; set; }
        [Required] public string? TituloAcademico { get; set; }
        public string? Universidad { get; set; }

        // --- Más datos faltantes ---
        public string? CedulaProfesional { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? TipoContrato { get; set; } // O usa string/enum si prefieres
        public int? Estatus { get; set; }      // O usa string/enum
        public decimal? Salario { get; set; }
    }
}
