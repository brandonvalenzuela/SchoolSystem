using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Evaluacion
{
    public class PeriodoEvaluacionDto
    {
        public int Id { get; set; }
        public int EscuelaId { get; set; }
        public int CicloEscolarId { get; set; }

        public string Nombre { get; set; } = "";
        public int Numero { get; set; }
        public bool Activo { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaLimiteCaptura { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        public decimal Porcentaje { get; set; }
        public string TipoPeriodo { get; set; } = "";

        // Para UI (opcional pero útil)
        public string CicloClave { get; set; } = ""; // "2025-2026"
    }
}
