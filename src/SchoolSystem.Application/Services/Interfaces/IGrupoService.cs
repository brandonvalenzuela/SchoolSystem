using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.DTOs.Grupos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IGrupoService
    {
        Task<GrupoDto> GetByIdAsync(int id);
        Task<PagedResult<GrupoDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateGrupoDto dto);
        Task UpdateAsync(int id, UpdateGrupoDto dto);
        Task DeleteAsync(int id);
    }
}
