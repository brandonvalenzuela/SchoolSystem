using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Alumnos
{
    /// <summary>
    /// DTO para representar la información de un alumno al ser leída.
    /// </summary>
    public class AlumnoDto
    {
        /// <summary>
        /// ID único del alumno.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID de la escuela a la que pertenece.
        /// </summary>
        public int EscuelaId { get; set; }

        /// <summary>
        /// Matrícula del alumno.
        /// </summary>
        public string Matricula { get; set; }

        /// <summary>
        /// Nombre(s) del alumno.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido paterno del alumno.
        /// </summary>
        public string ApellidoPaterno { get; set; }

        /// <summary>
        /// Apellido materno del alumno.
        /// </summary>
        public string ApellidoMaterno { get; set; }

        /// <summary>
        /// Nombre completo del alumno.
        /// </summary>
        public string NombreCompleto { get; set; }

        /// <summary>
        /// CURP del alumno.
        /// </summary>
        public string CURP { get; set; }

        /// <summary>
        /// Fecha de nacimiento del alumno.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Edad calculada del alumno.
        /// </summary>
        public int Edad { get; set; }

        /// <summary>
        /// Género del alumno (ej: "Masculino", "Femenino").
        /// </summary>
        public string Genero { get; set; }

        /// <summary>
        /// URL de la foto del alumno.
        /// </summary>
        public string FotoUrl { get; set; }

        /// <summary>
        /// Estatus actual del alumno (ej: "Activo", "Baja").
        /// </summary>
        public string Estatus { get; set; }

        /// <summary>
        /// Correo electrónico del alumno.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Teléfono de contacto del alumno.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Fecha de ingreso a la institución.
        /// </summary>
        public DateTime FechaIngreso { get; set; }
    }
}
