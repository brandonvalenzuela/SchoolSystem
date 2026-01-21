using AutoMapper;
using SchoolSystem.Application.DTOs.Evaluacion;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class PeriodoEvaluacionService : IPeriodoEvaluacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PeriodoEvaluacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PeriodoEvaluacionDto>> GetPorGrupoAsync(int grupoId, bool soloActivos = true)
        {
            var grupo = await _unitOfWork.Grupos.FirstOrDefaultAsync(g => g.Id == grupoId);
            if (grupo == null)
                return new();

            var cicloId = grupo.CicloEscolarId;
            var escuelaId = grupo.EscuelaId;

            var periodos = await _unitOfWork.PeriodoEvaluaciones.FindAsync(
                p => p.EscuelaId == escuelaId
                     && p.CicloEscolarId == cicloId
                     && (!soloActivos || p.Activo),
                p => p.Ciclo
            );

            return _mapper.Map<List<PeriodoEvaluacionDto>>(periodos.OrderBy(p => p.Numero));
        }
    }
}
