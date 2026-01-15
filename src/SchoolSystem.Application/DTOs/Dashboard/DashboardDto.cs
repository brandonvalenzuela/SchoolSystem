using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int TotalAlumnos { get; set; }
        public int TotalMaestros { get; set; }
        public int TotalGrupos { get; set; }

        // Finanzas
        public decimal IngresosMesActual { get; set; }
        public decimal CargosPendientesTotal { get; set; }
        public int PagosRealizadosHoy { get; set; }
        public ChartDto StudentsChart { get; set; }
        public ChartDto FinanceChart { get; set; }
    }
}
