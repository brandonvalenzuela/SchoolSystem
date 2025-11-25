using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Conducta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IRegistroConductaService
    {
        Task<RegistroConductaDto> GetByIdAsync(int id);
        Task<PagedResult<RegistroConductaDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateRegistroConductaDto dto);
        Task UpdateAsync(int id, UpdateRegistroConductaDto dto);
        Task DeleteAsync(int id);
    }
}
