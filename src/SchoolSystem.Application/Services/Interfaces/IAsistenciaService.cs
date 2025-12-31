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
        /// Registra la asistencia masiva para un grupo en una fecha específica.
        /// </summary>
        Task<int> CreateMasivoAsync(CreateAsistenciaMasivaDto dto);

        /// <summary>
        /// Obtiene la lista de asistencia de un grupo para una fecha específica (útil para verificar/editar).
        /// </summary>
        Task<List<AsistenciaDto>> GetByGrupoAndFechaAsync(int grupoId, DateTime fecha);

        /// <summary>
        /// Obtiene el historial de un alumno específico.
        /// </summary>
        Task<List<AsistenciaDto>> GetHistorialByAlumnoAsync(int alumnoId);
        
        /// <summary>
        /// Justifica una falta existente.
        /// </summary>
        Task JustificarFaltaAsync(int asistenciaId, JustificarFaltaDto dto, int usuarioId);

        /// <summary>
        /// Genera la data para el reporte mensual (sábana de asistencia).
        /// </summary>
        Task<List<ReporteMensualDto>> GetReporteMensualAsync(int grupoId, int mes, int anio);
    }
}
