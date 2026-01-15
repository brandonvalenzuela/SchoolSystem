using Microsoft.EntityFrameworkCore;
using SchoolSystem.Application.DTOs.Dashboard;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Enums.Academico;
using SchoolSystem.Domain.Enums.Finanzas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardDto> GetStatsAsync()
        {
            var stats = new DashboardDto();
            var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var hoy = DateTime.Now.Date;

            // 1. Conteos básicos (Usamos CountAsync si tu repo lo soporta, o GetAll y Count)
            var alumnos = await _unitOfWork.Alumnos.FindAsync(a => a.Estatus == EstatusAlumno.Activo && !a.IsDeleted);
            stats.TotalAlumnos = alumnos.Count();

            var maestros = await _unitOfWork.Maestros.FindAsync(m => m.Estatus == EstatusLaboral.Activo && !m.IsDeleted);
            stats.TotalMaestros = maestros.Count();

            var grupos = await _unitOfWork.Grupos.FindAsync(g => g.Activo && !g.IsDeleted);
            stats.TotalGrupos = grupos.Count();

            // 2. Finanzas - Ingresos del mes
            var pagosMes = await _unitOfWork.Pagos.FindAsync(p =>
                p.FechaPago >= fechaInicioMes &&
                !p.Cancelado &&
                !p.IsDeleted);
            stats.IngresosMesActual = pagosMes.Sum(p => p.Monto);

            // 3. Finanzas - Pagos de hoy
            stats.PagosRealizadosHoy = pagosMes.Count(p => p.FechaPago.Date == hoy);

            // 4. Finanzas - Pendientes totales (Cartera vencida + por cobrar)
            var cargosPendientes = await _unitOfWork.Cargos.FindAsync(c =>
                (c.Estatus == EstatusCargo.Pendiente || c.Estatus == EstatusCargo.Vencido || c.Estatus == EstatusCargo.Parcial) &&
                !c.IsDeleted);
            stats.CargosPendientesTotal = cargosPendientes.Sum(c => c.SaldoPendiente);



            // --- LÓGICA GRÁFICA 1: MOVIMIENTOS DE ALUMNOS (Últimos 5 años) ---
            var anioActual = DateTime.Now.Year;
            var ultimosAnios = Enumerable.Range(anioActual - 4, 5).ToList();

            var dataIngresos = new List<double>();
            var dataBajas = new List<double>();     // Deserciones
            var dataEgresados = new List<double>(); // Graduados
            var labelsAnios = new List<string>();

            foreach (var anio in ultimosAnios)
            {
                labelsAnios.Add(anio.ToString());

                // 1. NUEVOS INGRESOS
                var ingresos = await _unitOfWork.Alumnos.CountAsync(a =>
                    a.FechaIngreso.Year == anio &&
                    !a.IsDeleted);
                dataIngresos.Add(ingresos);

                // 2. BAJAS (Deserciones)
                // Buscamos fecha y que el estatus sea explícitamente BAJA
                var bajas = await _unitOfWork.Alumnos.CountAsync(a =>
                    a.FechaBaja.HasValue &&
                    a.FechaBaja.Value.Year == anio &&
                    a.Estatus == EstatusAlumno.Baja && // <-- Filtro clave
                    !a.IsDeleted);
                dataBajas.Add(bajas);

                // 3. EGRESADOS (Éxito)
                // Buscamos fecha y que el estatus sea EGRESADO
                var egresados = await _unitOfWork.Alumnos.CountAsync(a =>
                    a.FechaBaja.HasValue &&
                    a.FechaBaja.Value.Year == anio &&
                    a.Estatus == EstatusAlumno.Egresado && // <-- Filtro clave
                    !a.IsDeleted);
                dataEgresados.Add(egresados);
            }

            stats.StudentsChart = new ChartDto
            {
                Labels = labelsAnios,
                Series = new List<ChartSeriesDto>
        {
            new ChartSeriesDto { Name = "Nuevos Ingresos", Data = dataIngresos.ToArray() },
            new ChartSeriesDto { Name = "Egresados", Data = dataEgresados.ToArray() }, // Verde (Éxito)
            new ChartSeriesDto { Name = "Bajas", Data = dataBajas.ToArray() }          // Rojo (Alerta)
        }
            };

            // --- LÓGICA GRÁFICA 2: FINANZAS (Últimos 6 meses) ---
            // (Implementación simplificada para el ejemplo)
            var meses = new List<string>();
            var ingresosReales = new List<double>();

            for (int i = 5; i >= 0; i--)
            {
                var fecha = DateTime.Now.AddMonths(-i);
                meses.Add(fecha.ToString("MMM"));

                var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
                var finMes = inicioMes.AddMonths(1).AddSeconds(-1);

                var totalMes = await _unitOfWork.Pagos.GetQueryable()
                    .Where(p => p.FechaPago >= inicioMes && p.FechaPago <= finMes && !p.Cancelado && !p.IsDeleted)
                    .SumAsync(p => p.Monto);

                ingresosReales.Add((double)totalMes);
            }

            stats.FinanceChart = new ChartDto
            {
                Labels = meses,
                Series = new List<ChartSeriesDto>
                {
                    new ChartSeriesDto { Name = "Ingresos Netos", Data = ingresosReales.ToArray() }
                }
            };


            return stats;
        }
    }
}
