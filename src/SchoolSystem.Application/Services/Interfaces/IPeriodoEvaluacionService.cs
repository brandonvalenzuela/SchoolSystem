using SchoolSystem.Application.DTOs.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface IPeriodoEvaluacionService
    {
        Task<List<PeriodoEvaluacionDto>> GetPorGrupoAsync(int grupoId, bool soloActivos = true);
    }
}
