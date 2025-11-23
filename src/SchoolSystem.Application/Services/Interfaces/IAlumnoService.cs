using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.DTOs.Asistencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IAlumnoService
    {
        Task<AlumnoDto> GetByIdAsync(int id);
        Task<PagedResult<AlumnoDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateAlumnoDto dto);
        Task UpdateAsync(int id, UpdateAlumnoDto dto);
        Task DeleteAsync(int id);
    }
}
