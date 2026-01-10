using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Filtros
{
    public class AlumnoFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Filtros opcionales
        public string? TerminoBusqueda { get; set; } // Nombre, Apellido, Matrícula
        public string? Estatus { get; set; } // "Activo", "Baja"
        public string? GrupoId { get; set; }
    }
}
