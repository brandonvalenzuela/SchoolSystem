using SchoolSystem.Application.Common.Interfaces;
using SchoolSystem.Domain.Enums.Academico;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Alumnos
{
    /// <summary>
    /// DTO para actualizar la información de un alumno existente.
    /// </summary>
    public class UpdateAlumnoDto : IPersonaDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        [StringLength(50, ErrorMessage = "La matrícula no puede exceder los 50 caracteres.")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido paterno no puede exceder los 100 caracteres.")]
        public string ApellidoPaterno { get; set; }

        [StringLength(100, ErrorMessage = "El apellido materno no puede exceder los 100 caracteres.")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        public Genero Genero { get; set; }

        [StringLength(18, ErrorMessage = "El CURP debe tener 18 caracteres.")]
        public string CURP { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(300)]
        public string Direccion { get; set; }

        [StringLength(500)]
        public string FotoUrl { get; set; }

        // El estatus podría ser modificado en una actualización
        [Required]
        public EstatusAlumno Estatus { get; set; }

        // Información médica y de contacto de emergencia (simplificada como opcional)
        public string TipoSangre { get; set; }
        public string Alergias { get; set; }
        public string CondicionesMedicas { get; set; }
        public string Medicamentos { get; set; }
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public string ContactoEmergenciaRelacion { get; set; }
    }
}
