using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.ConfiguracionEscuela;
using SchoolSystem.Application.DTOs.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IEscuelaService
    {
        Task<EscuelaDto> GetByIdAsync(int id);
        Task<PagedResult<EscuelaDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateEscuelaDto dto);
        Task UpdateAsync(int id, UpdateEscuelaDto dto);
        Task DeleteAsync(int id);
        // Agregar este método a la interfaz
        Task<ResumenEscuelaDto> GetResumenAsync(int id);
        Task<ConfiguracionEscuelaDto> GetConfiguracionAsync(int escuelaId);
        Task UpdateConfiguracionAsync(int escuelaId, UpdateConfiguracionEscuelaDto dto);
    }
}
