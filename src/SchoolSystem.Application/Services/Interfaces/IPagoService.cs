using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Finanzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IPagoService
    {
        // Consultas
        Task<PagedResult<PagoDto>> GetPagosAsync(int page, int size);
        Task<List<CargoDto>> GetCargosPendientesPorAlumnoAsync(int alumnoId);

        // Operaciones
        Task<int> RegistrarPagoAsync(CreatePagoDto dto);
        Task CancelarPagoAsync(int pagoId, string motivo, int usuarioId);

        // Generación de Cargos (Ej: Correr proceso mensual)
        Task GenerarCargosMensualesAsync(int escuelaId, int mes, int anio);
    }
}
