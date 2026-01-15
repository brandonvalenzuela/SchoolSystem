namespace SchoolSystem.Web.Models
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

        // Gráficas (Nuevas propiedades)
        public ChartDto StudentsChart { get; set; }
        public ChartDto FinanceChart { get; set; }
    }
}
