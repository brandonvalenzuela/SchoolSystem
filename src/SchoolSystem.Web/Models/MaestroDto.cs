namespace SchoolSystem.Web.Models
{
    public class MaestroDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EscuelaId { get; set; }

        // Datos de Usuario
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string FotoUrl { get; set; }
        public bool Activo { get; set; }

        // FALTABAN ESTOS POSIBLEMENTE:
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }

        // Datos de Maestro
        public string NumeroEmpleado { get; set; }
        public string Especialidad { get; set; }
        public string TituloAcademico { get; set; }
        public string Universidad { get; set; }
        public DateTime? FechaIngreso { get; set; }

        // ¡ESTE ES EL QUE FALTABA SEGURAMENTE!
        public string CedulaProfesional { get; set; }

        public int CantidadMaterias { get; set; }
    }
}
