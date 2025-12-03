using AutoMapper;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Domain.Entities.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class AlumnoProfile : Profile
    {
        public AlumnoProfile()
        {
            // Entidad → DTO de lectura
            CreateMap<Alumno, AlumnoDto>()
                .ForMember(dest => dest.Estatus,
                           opt => opt.MapFrom(src => src.Estatus.ToString()))
                .ForMember(dest => dest.Genero,
                           opt => opt.MapFrom(src => src.Genero.ToString()));

            // DTO de creación → Entidad
            CreateMap<CreateAlumnoDto, Alumno>();

            // DTO de actualización → Entidad
            CreateMap<UpdateAlumnoDto, Alumno>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
