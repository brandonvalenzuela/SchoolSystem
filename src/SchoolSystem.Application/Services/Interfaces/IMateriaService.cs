using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.DTOs.Materias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IMateriaService
    {
        Task<MateriaDto> GetByIdAsync(int id);
        Task<PagedResult<MateriaDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateMateriaDto dto);
        Task UpdateAsync(int id, UpdateMateriaDto dto);
        Task DeleteAsync(int id);
    }
}
