using AutoMapper;
using SchoolSystem.Application.DTOs.Grados;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class GradoProfile : Profile
    {
        public GradoProfile()
        {
            CreateMap<Grado, GradoDto>()
                .ForMember(dest => dest.NombreNivelEducativo, opt => opt.MapFrom(src => src.NivelEducativo.Nombre));

            CreateMap<CreateGradoDto, Grado>();
            CreateMap<UpdateGradoDto, Grado>();
        }
    }
}
