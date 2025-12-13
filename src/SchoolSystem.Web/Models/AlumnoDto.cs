namespace SchoolSystem.Web.Models
{
    public class AlumnoDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public string CURP { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string FotoUrl { get; set; }
        public string Estatus { get; set; }
        public string Email { get; set; }

        public string Telefono { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
