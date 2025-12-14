using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateMaestroDto
    {
        public int EscuelaId { get; set; } = 1;

        // --- Datos de Usuario ---
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }

        // --- Datos Faltantes que causaban error ---
        public string CedulaProfesional { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }

        // --- Datos de Maestro ---
        [Required(ErrorMessage = "El No. Empleado es obligatorio")]
        public string NumeroEmpleado { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public string Especialidad { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string TituloAcademico { get; set; }

        public string Universidad { get; set; }
    }
}
