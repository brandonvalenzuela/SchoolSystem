using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateAlumnoDto
    {
        public int Id { get; set; }

        [Required]
        public string Matricula { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public int Genero { get; set; }

        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int Estatus { get; set; } // En tu API es Enum, aquí puede ser int
    }
}
