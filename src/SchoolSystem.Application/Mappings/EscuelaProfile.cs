using AutoMapper;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Domain.Entities.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class EscuelaProfile : Profile
    {
        public EscuelaProfile()
        {
            // Entity -> DTO
            CreateMap<Escuela, EscuelaDto>()
                .ForMember(dest => dest.TipoPlan, 
                           opt => opt.MapFrom(src => src.TipoPlan.ToString()))
                .ForMember(dest => dest.TieneSuscripcionVigente,
                           opt => opt.MapFrom(src => src.TieneSuscripcionVigente()));

            // CreateDTO -> Entity
            CreateMap<CreateEscuelaDto, Escuela>();

            // UpdateDTO -> Entity
            CreateMap<UpdateEscuelaDto, Escuela>();
        }
    }
}
