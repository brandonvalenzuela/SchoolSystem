using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Maestros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IMaestroService
    {
        Task<MaestroDto> GetByIdAsync(int id);
        Task<PagedResult<MaestroDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateMaestroDto dto);
        Task UpdateAsync(int id, UpdateMaestroDto dto);
        Task DeleteAsync(int id);

    }
}
