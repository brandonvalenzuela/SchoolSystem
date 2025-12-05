using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Calificacion
{
    public class MateriaBoletaDto
    {
        public string NombreMateria { get; set; }
        public string Profesor { get; set; }

        // Diccionario: Clave = Nombre del Periodo (ej: "Bimestre 1"), Valor = Calificación
        public Dictionary<string, decimal?> CalificacionesPorPeriodo { get; set; }

        public decimal PromedioMateria { get; set; }
    }
}
