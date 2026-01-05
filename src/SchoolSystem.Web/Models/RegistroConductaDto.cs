namespace SchoolSystem.Web.Models
{
    public class RegistroConductaDto
    {
        public int Id { get; set; }
        public string NombreAlumno { get; set; }
        public string NombreMaestro { get; set; }
        public string Tipo { get; set; } // Positiva, Negativa
        public string Categoria { get; set; }
        public string Gravedad { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Puntos { get; set; }
        public DateTime FechaHoraIncidente { get; set; }
        public string Estado { get; set; }
    }
}
