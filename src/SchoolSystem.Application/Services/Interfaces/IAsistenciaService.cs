using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Application.DTOs.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IAsistenciaService
    {
        Task<AsistenciaDto> GetByIdAsync(int id);
        Task<PagedResult<AsistenciaDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateAsistenciaDto dto);
        Task UpdateAsync(int id, UpdateAsistenciaDto dto);
        Task DeleteAsync(int id);
        /// <summary>
        /// Registra la asistencia de todo un grupo para una fecha específica.
        /// </summary>
        /// <returns>Cantidad de registros creados.</returns>
        Task<int> CreateMasivoAsync(CreateAsistenciaMasivaDto dto);
        Task<List<AsistenciaDto>> GetByGrupoAndFechaAsync(int grupoId, DateTime fecha);
        Task<List<AsistenciaDto>> GetHistorialByAlumnoAsync(int alumnoId);
        Task JustificarFaltaAsync(int asistenciaId, JustificarFaltaDto dto, int usuarioId);
        Task<List<ReporteMensualDto>> GetReporteMensualAsync(int grupoId, int mes, int anio);
    }
}
