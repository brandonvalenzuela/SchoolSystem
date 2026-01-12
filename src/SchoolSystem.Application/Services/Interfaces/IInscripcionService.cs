using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.DTOs.Inscripciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IInscripcionService
    {
        Task<InscripcionDto> GetByIdAsync(int id);
        Task<PagedResult<InscripcionDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateInscripcionDto dto);
        Task UpdateAsync(int id, UpdateInscripcionDto dto);
        Task DeleteAsync(int id);

        // --- NUEVO MÉTODO ---
        /// <summary>
        /// Cambia a un alumno de grupo, validando cupo y reglas de negocio.
        /// </summary>
        Task CambiarDeGrupoAsync(int inscripcionId, int nuevoGrupoId, string motivo, int usuarioId);
        Task<List<InscripcionDto>> GetAlumnosPorGrupoAsync(int grupoId);
        Task<List<InscripcionDto>> GetHistorialPorAlumnoAsync(int alumnoId);
    }
}
