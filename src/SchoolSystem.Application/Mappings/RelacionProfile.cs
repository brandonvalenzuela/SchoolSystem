using AutoMapper;
using SchoolSystem.Application.DTOs.Academicos;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class RelacionProfile : Profile
    {
        public RelacionProfile()
        {
            CreateMap<CreateAlumnoPadreDto, AlumnoPadre>()
                .ForMember(d => d.Relacion, o => o.MapFrom(s => Enum.Parse<RelacionFamiliar>(s.Relacion)));

            CreateMap<AlumnoPadre, AlumnoPadreDto>()
                .ForMember(d => d.NombreAlumno, o => o.MapFrom(s => s.Alumno.NombreCompleto))
                .ForMember(d => d.NombrePadre, o => o.MapFrom(s => s.Padre.Usuario.NombreCompleto))
                .ForMember(d => d.Relacion, o => o.MapFrom(s => s.Relacion.ToString()));
        }
    }
}
