
namespace SchoolSystem.Web.Models
{
    public class AlumnoPadreDto
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public int PadreId { get; set; }
        public string NombrePadre { get; set; }
        public string Relacion { get; set; }
        public bool EsTutorPrincipal { get; set; }
        public bool ViveConAlumno { get; set; }
        public bool AutorizadoRecoger { get; set; }
    }
}
