using SchoolSystem.Application.DTOs.Alumnos;
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
        Task<int> CreateAsync(CreateAlumnoDto dto);
        Task UpdateAsync(UpdateAlumnoDto dto);
        Task DeleteAsync(int id);
    }
}
