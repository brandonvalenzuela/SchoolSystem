using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Escuelas
{
    /// <summary>
    /// DTO para representar la información de una escuela al ser leída.
    /// </summary>
    public class EscuelaDto
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string RazonSocial { get; set; }

        public string RFC { get; set; }

        // Contacto
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string SitioWeb { get; set; }
        public string LogoUrl { get; set; }

        // Suscripción
        public string TipoPlan { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public bool Activo { get; set; }
        public bool TieneSuscripcionVigente { get; set; } // Propiedad calculada

        // Límites
        public int? MaxAlumnos { get; set; }
        public int? MaxMaestros { get; set; }
        public int? EspacioAlmacenamiento { get; set; }
    }
}
