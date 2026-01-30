using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Materias
{
    /// <summary>
    /// DTO para actualizar una materia existente.
    /// </summary>
    public class UpdateMateriaDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La clave de la materia es obligatoria.")]
        [StringLength(20, ErrorMessage = "La clave no puede exceder los 20 caracteres.")]
        public string? Clave { get; set; }

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        public AreaAcademica Area { get; set; }

        [Required]
        public TipoMateria Tipo { get; set; }

        public IconoMateria? Icono { get; set; }

        /// <summary>
        /// Color en formato hexadecimal (#RGB o #RRGGBB).
        /// Ejemplo: "#FF5" o "#FF5733"
        /// Nullable: permite omitir color
        /// </summary>
        [StringLength(7, MinimumLength = 4, ErrorMessage = "El ColorHex debe ser #RGB (4) o #RRGGBB (7).")]
        [RegularExpression(@"^#([A-Fa-f0-9]{3}|[A-Fa-f0-9]{6})$", ErrorMessage = "ColorHex debe ser #RGB o #RRGGBB con valores hexadecimales válidos.")]
        public string? ColorHex { get; set; }

        [Range(1, 5, ErrorMessage = "El nivel de dificultad debe estar entre 1 y 5.")]
        public int? NivelDificultad { get; set; }

        public bool RequiereMateriales { get; set; }
        public string? MaterialesRequeridos { get; set; }

        public bool RequiereInstalacionesEspeciales { get; set; }
        public string? InstalacionesRequeridas { get; set; }

        public string? Objetivos { get; set; }
        public string? Competencias { get; set; }
        public string? ContenidoTematico { get; set; }
        public string? Bibliografia { get; set; }

        public bool Activo { get; set; }
    }
}
