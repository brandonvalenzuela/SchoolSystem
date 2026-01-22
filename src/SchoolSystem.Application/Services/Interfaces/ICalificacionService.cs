using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificacion;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.DTOs.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface ICalificacionService
    {
        Task<CalificacionDto> GetByIdAsync(int id);
        Task<PagedResult<CalificacionDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateCalificacionDto dto);
        Task UpdateAsync(int id, UpdateCalificacionDto dto);
        Task DeleteAsync(int id);

        // 5. Carga Masiva
        Task<CalificacionMasivaResultadoDto> CreateMasivoAsync(CreateCalificacionMasivaDto dto);

        // 3. Reporte de Boleta
        Task<BoletaDto> GetBoletaAsync(int alumnoId, string cicloEscolar);
    }
}
