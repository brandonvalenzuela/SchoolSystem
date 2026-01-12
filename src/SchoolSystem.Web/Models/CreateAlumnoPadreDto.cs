using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class CreateAlumnoPadreDto
    {
        [Required(ErrorMessage = "Seleccione un alumno")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "Seleccione un padre")]
        public int PadreId { get; set; }

        [Required]
        public string Relacion { get; set; } = "Padre"; // Padre, Madre, Tutor, Abuelo

        public bool EsTutorPrincipal { get; set; }
        public bool ViveConAlumno { get; set; }
        public bool AutorizadoRecoger { get; set; }
        public bool RecibeNotificaciones { get; set; } = true;
    }
}
