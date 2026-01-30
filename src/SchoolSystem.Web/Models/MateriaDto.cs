namespace SchoolSystem.Web.Models
{
    public class MateriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Area { get; set; }
        public string Tipo { get; set; }

        /// <summary>
        /// Descripción de la materia (máx 1000 caracteres)
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Color en formato hexadecimal (#RGB o #RRGGBB)
        /// Ejemplo: "#FF5733"
        /// Nullable: fallback a "#9E9E9E" si es null
        /// </summary>
        public string? ColorHex { get; set; }

        public bool Activo { get; set; }
    }
}
