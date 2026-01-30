using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Models
{
    public class UpdateMateriaDto
    {
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Clave { get; set; }

        public int Area { get; set; }

        public int Tipo { get; set; }

        /// <summary>
        /// Color en formato hexadecimal (#RGB o #RRGGBB)
        /// Ejemplo: "#FF5733"
        /// Nullable
        /// </summary>
        [StringLength(7, MinimumLength = 4, ErrorMessage = "ColorHex debe ser #RGB (4) o #RRGGBB (7).")]
        [RegularExpression(@"^#([A-Fa-f0-9]{3}|[A-Fa-f0-9]{6})$", ErrorMessage = "ColorHex debe ser #RGB o #RRGGBB con valores hexadecimales.")]
        public string? ColorHex { get; set; }

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public bool RequiereMateriales { get; set; }

        [StringLength(1000)]
        public string? MaterialesRequeridos { get; set; }

        public bool RequiereInstalacionesEspeciales { get; set; }

        [StringLength(500)]
        public string? InstalacionesRequeridas { get; set; }
    }
}
