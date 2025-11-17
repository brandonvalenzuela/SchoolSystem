using SchoolSystem.Domain.Enums.Escuelas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Escuelas
{
    /// <summary>
    /// DTO para actualizar la información de una escuela existente.
    /// </summary>
    public class UpdateEscuelaDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de la escuela es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la escuela es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(300)]
        public string RazonSocial { get; set; }

        [StringLength(13, ErrorMessage = "El RFC debe tener entre 12 y 13 caracteres.")]
        public string RFC { get; set; }

        // Contacto
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(500)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; }

        [Required]
        [StringLength(100)]
        public string Estado { get; set; }

        [Required]
        [StringLength(20)]
        public string CodigoPostal { get; set; }

        [Required]
        [StringLength(100)]
        public string Pais { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [Url]
        [StringLength(300)]
        public string SitioWeb { get; set; }

        [Url]
        [StringLength(500)]
        public string LogoUrl { get; set; }

        // Suscripción
        [Required]
        public TipoPlan TipoPlan { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public bool Activo { get; set; }

        // Límites del Plan
        [Range(1, int.MaxValue, ErrorMessage = "El límite de alumnos debe ser un número positivo.")]
        public int? MaxAlumnos { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El límite de maestros debe ser un número positivo.")]
        public int? MaxMaestros { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El espacio de almacenamiento debe ser un número positivo.")]
        public int? EspacioAlmacenamiento { get; set; }
    }
}
