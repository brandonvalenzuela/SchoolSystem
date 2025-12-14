using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateAlumnoDto
    {
        [Required(ErrorMessage = "La escuela es obligatoria")]
        public int EscuelaId { get; set; } = 1; // Valor por defecto temporal (si solo tienes una escuela)

        [Required(ErrorMessage = "La matrícula es obligatoria")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime? FechaNacimiento { get; set; } = DateTime.Now.AddYears(-5);

        public int Genero { get; set; } = 1; // 1 = Masculino (según tu Enum)

        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
