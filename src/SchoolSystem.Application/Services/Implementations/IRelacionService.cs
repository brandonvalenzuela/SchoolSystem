using SchoolSystem.Application.DTOs.Academicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public interface IRelacionService
    {
        Task<int> VincularAsync(CreateAlumnoPadreDto dto);
        Task DesvincularAsync(int id);
        Task<List<AlumnoPadreDto>> GetPorPadreAsync(int padreId);
        Task<List<AlumnoPadreDto>> GetPorAlumnoAsync(int alumnoId);
    }
}
