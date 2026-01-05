using SchoolSystem.Application.DTOs.Medico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<ExpedienteMedicoDto> GetByAlumnoIdAsync(int alumnoId);
        Task<int> CreateAsync(CreateExpedienteDto dto);
        Task UpdateAsync(int id, UpdateExpedienteDto dto);
    }
}
