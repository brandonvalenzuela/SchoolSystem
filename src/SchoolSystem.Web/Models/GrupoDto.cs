namespace SchoolSystem.Web.Models
{
    public class GrupoDto
    {
        public int Id { get; set; }
        public int GradoId { get; set; }
        public int EscuelaId { get; set; }
        public string NombreGrado { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public int CicloEscolarId { get; set; }
        public string CicloEscolarClave { get; set; } = "";
        public int CapacidadMaxima { get; set; }
        public int CantidadAlumnos { get; set; }
        public int? MaestroTitularId { get; set; }
        public string NombreMaestroTitular { get; set; }
        public string Aula { get; set; }
        public string Turno { get; set; }
        public bool Activo { get; set; }
    }
}
