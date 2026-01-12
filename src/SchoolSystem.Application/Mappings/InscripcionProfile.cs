using AutoMapper;
using SchoolSystem.Application.DTOs.Inscripciones;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class InscripcionProfile : Profile
    {
        public InscripcionProfile()
        {
            CreateMap<Inscripcion, InscripcionDto>()
                .ForMember(dest => dest.NombreCompletoAlumno, opt => opt.MapFrom(src => src.Alumno.NombreCompleto))
                .ForMember(dest => dest.NombreCompletoGrupo, opt => opt.MapFrom(src => src.Grupo.NombreCompleto))
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus.HasValue ? src.Estatus.ToString() : "Desconocido"))
                .ForMember(dest => dest.Matricula, opt => opt.MapFrom(src => src.Alumno.Matricula));

            CreateMap<CreateInscripcionDto, Inscripcion>();
            CreateMap<UpdateInscripcionDto, Inscripcion>()
                  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
