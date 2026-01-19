using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificacion
{
    public class BoletaDto
    {
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; }
        public string Matricula { get; set; }
        public string Grupo { get; set; }
        public int CicloEscolarId { get; set; }
        public string CicloEscolarClave { get; set; } = "";
        public decimal PromedioGeneral { get; set; }

        // Lista de materias con sus calificaciones por periodo
        public List<MateriaBoletaDto> Materias { get; set; }
    }
}
