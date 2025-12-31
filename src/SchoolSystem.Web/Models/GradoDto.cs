namespace SchoolSystem.Web.Models
{
    public class GradoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // "1°", "2°"
        public string NombreNivelEducativo { get; set; } // "Primaria"
        public string NombreCompleto { get; set; } // "1° de Primaria"
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public int CantidadGruposActivos { get; set; }
    }
}
