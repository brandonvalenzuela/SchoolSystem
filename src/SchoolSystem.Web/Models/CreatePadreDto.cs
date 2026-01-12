using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreatePadreDto
    {
        public int EscuelaId { get; set; } = 1;

        [Required(ErrorMessage = "Debe seleccionar al alumno")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El email es vital para la activación")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; }

        public string? Telefono { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Especifique el parentesco")]
        public string Relacion { get; set; } = "Padre"; // Padre, Madre, Tutor...

        public string? Ocupacion { get; set; }
        public string? LugarTrabajo { get; set; }
    }
}
