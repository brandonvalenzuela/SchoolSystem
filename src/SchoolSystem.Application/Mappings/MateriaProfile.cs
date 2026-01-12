using AutoMapper;
using SchoolSystem.Application.DTOs.Materias;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class MateriaProfile : Profile
    {
        public MateriaProfile()
        {
            CreateMap<Materia, MateriaDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.ToString()))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
                .ForMember(dest => dest.Icono, opt => opt.MapFrom(src => src.Icono.HasValue ? src.Icono.ToString() : "Default"));

            CreateMap<CreateMateriaDto, Materia>();
            CreateMap<UpdateMateriaDto, Materia>()
                  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
