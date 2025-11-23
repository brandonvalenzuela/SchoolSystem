using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.DTOs.Padres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IPadreService
    {
        Task<PadreDto> GetByIdAsync(int id);
        Task<PagedResult<PadreDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreatePadreDto dto);
        Task UpdateAsync(int id, UpdatePadreDto dto);
        Task DeleteAsync(int id);
    }
}
