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

            return stats;
        }
    }
}
