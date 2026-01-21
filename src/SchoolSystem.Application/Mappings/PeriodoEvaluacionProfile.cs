using AutoMapper;
using SchoolSystem.Application.DTOs.Evaluacion;
using SchoolSystem.Domain.Entities.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    internal class PeriodoEvaluacionProfile : Profile
    {
        public PeriodoEvaluacionProfile() 
        {
            CreateMap<PeriodoEvaluacion, PeriodoEvaluacionDto>()
                .ForMember(d => d.CicloClave, o => o.MapFrom(s => s.Ciclo.Clave));
        }
    }
}
