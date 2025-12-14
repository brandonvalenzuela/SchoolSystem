namespace SchoolSystem.Web.Models
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } // "Director", "Maestro", etc.
        public string NombreCompleto { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
    }
}
