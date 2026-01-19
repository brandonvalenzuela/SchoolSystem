using AutoMapper;
using SchoolSystem.Application.DTOs.Grupos;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class GrupoProfile : Profile
    {
        public GrupoProfile()
        {
            CreateMap<Grupo, GrupoDto>()
                .ForMember(dest => dest.NombreGrado, opt => opt.MapFrom(src => src.Grado.NombreCompleto))
                .ForMember(dest => dest.NombreMaestroTitular, opt => opt.MapFrom(src => src.MaestroTitular != null ? src.MaestroTitular.Usuario.NombreCompleto : "Sin asignar"))
                .ForMember(dest => dest.Turno, opt => opt.MapFrom(src => src.Turno.HasValue ? src.Turno.ToString() : "No definido"))
                .ForMember(dest => dest.CicloEscolarId, o => o.MapFrom(s => s.CicloEscolarId))
                .ForMember(dest => dest.CicloEscolarClave, o => o.MapFrom(s => s.Ciclo.Clave));

            CreateMap<CreateGrupoDto, Grupo>()
                .ForMember(d => d.Ciclo, o => o.Ignore())
                .ForMember(d => d.CicloEscolarId, o => o.Ignore());

            CreateMap<UpdateGrupoDto, Grupo>()
                .ForMember(d => d.Ciclo, o => o.Ignore())
                .ForMember(d => d.CicloEscolarId, o => o.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
