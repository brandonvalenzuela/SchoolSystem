using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.ConfiguracionEscuela
{
    public class UpdateConfiguracionEscuelaDto
    {
        [Required]
        public int EscuelaId { get; set; }

        [StringLength(300)]
        public string? NombreInstitucion { get; set; }

        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Formato de color inválido")]
        public string? ColorPrimario { get; set; }

        [Range(0, 100)]
        public decimal? CalificacionMinimaAprobatoria { get; set; }

        // ... Agrega aquí cualquier otro campo que quieras permitir que el director edite
    }
}
