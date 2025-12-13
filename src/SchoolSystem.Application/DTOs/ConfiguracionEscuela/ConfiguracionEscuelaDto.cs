using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.ConfiguracionEscuela
{
    public class ConfiguracionEscuelaDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }

        // Identidad
        public string NombreInstitucion { get; set; }
        public string Lema { get; set; }
        public string LogoUrl { get; set; }
        public string ColorPrimario { get; set; }
        public string ColorSecundario { get; set; }

        // Académico
        public decimal CalificacionMinimaAprobatoria { get; set; }
        public decimal CalificacionMaxima { get; set; }
        public bool NotificarCalificacionesAutomaticamente { get; set; }

        // Contacto (Resumen)
        public string Email { get; set; }
        public string SitioWeb { get; set; }
    }
}
