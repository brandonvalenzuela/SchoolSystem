namespace SchoolSystem.Web.Models
{
    public class PadreDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EscuelaId { get; set; }

        // --- Datos del Usuario (Mapeados desde el Backend) ---
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string FotoUrl { get; set; }
        public bool Activo { get; set; } // <-- Esta faltaba

        // --- Datos Específicos del Padre ---
        public string Ocupacion { get; set; }
        public string LugarTrabajo { get; set; }
        public string TelefonoTrabajo { get; set; }
        public string NivelEstudios { get; set; }
        public string EstadoCivil { get; set; }

        // --- Preferencias de Notificación ---
        public bool AceptaSMS { get; set; }
        public bool AceptaEmail { get; set; }
        public bool AceptaPush { get; set; }

        // --- Información Agregada ---
        public int CantidadHijos { get; set; }
        public List<HijoAsociadoDto> Hijos { get; set; }
    }
}
