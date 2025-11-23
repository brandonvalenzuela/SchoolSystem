using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.DTOs.Grados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IGradoService
    {
        Task<GradoDto> GetByIdAsync(int id);
        Task<PagedResult<GradoDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateGradoDto dto);
        Task UpdateAsync(int id, UpdateGradoDto dto);
        Task DeleteAsync(int id);
    }
}
