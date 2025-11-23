using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetByIdAsync(int id);
        Task<PagedResult<UsuarioDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CreateAsync(CreateUsuarioDto dto);
        Task UpdateAsync(int id, UpdateUsuarioDto dto);
        Task DeleteAsync(int id);

        // Método específico extra
        Task<UsuarioDto> GetByUsernameAsync(string username);
    }
}
