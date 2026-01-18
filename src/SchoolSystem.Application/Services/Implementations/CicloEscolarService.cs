using SchoolSystem.Application.DTOs.Ciclos;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class CicloEscolarService : ICicloEscolarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CicloEscolarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CicloEscolarActualDto?> GetActualAsync(int escuelaId)
        {
            // 1) Intentar EsActual = true
            var actual = await _unitOfWork.CicloEscolares.FirstOrDefaultAsync(x =>
                x.EscuelaId == escuelaId && x.EsActual);

            // 2) Fallback: el más reciente por FechaInicio/Id si no hay marcado actual
            if (actual == null)
            {
                // Si tienes FechaInicio, es mejor; si no, usamos Id desc
                var q = _unitOfWork.CicloEscolares.GetQueryable()
                    .Where(x => x.EscuelaId == escuelaId);

                actual = q
                    .OrderByDescending(x => x.FechaInicio.HasValue) // prioriza los que sí tienen fecha
                    .ThenByDescending(x => x.FechaInicio)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault();
            }

            if (actual == null)
                return null;

            return new CicloEscolarActualDto
            {
                Id = actual.Id,
                Clave = actual.Clave,
                Nombre = actual.Nombre
            };
        }
    }
}
