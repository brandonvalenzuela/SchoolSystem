using AutoMapper;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Domain.Entities.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class CalificacionProfile : Profile
    {
        public CalificacionProfile()
        {
            CreateMap<Calificacion, CalificacionDto>()
                .ForMember(dest => dest.NombreCompletoAlumno, opt => opt.MapFrom(src => src.Alumno != null ? src.Alumno.NombreCompleto : "Desconocido"))
                .ForMember(dest => dest.NombreMateria, opt => opt.MapFrom(src => src.Materia.Nombre))
                .ForMember(dest => dest.NombreGrupo, opt => opt.MapFrom(src => src.Grupo.Nombre))
                .ForMember(dest => dest.NombrePeriodo, opt => opt.MapFrom(src => src.Periodo.Nombre))
                .ForMember(dest => dest.NombreMaestroCaptura, opt => opt.MapFrom(src => src.MaestroCaptura != null ? src.MaestroCaptura.Usuario.NombreCompleto : "Sistema"));

            CreateMap<CreateCalificacionDto, Calificacion>();
            CreateMap<UpdateCalificacionDto, Calificacion>();
        }
    }
}
