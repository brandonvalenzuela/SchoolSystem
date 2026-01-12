using AutoMapper;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Domain.Entities.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class AsistenciaProfile : Profile
    {
        public AsistenciaProfile()
        {
            // Entity -> DTO
            CreateMap<Asistencia, AsistenciaDto>()
                .ForMember(dest => dest.NombreCompletoAlumno, opt => opt.MapFrom(src => src.Alumno.NombreCompleto))
                .ForMember(dest => dest.NombreGrupo, opt => opt.MapFrom(src => src.Grupo.Nombre))
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus.ToString()))
                .ForMember(dest => dest.HoraEntrada, opt => opt.MapFrom(src => src.HoraEntrada.HasValue ? src.HoraEntrada.Value.ToString(@"hh\:mm") : null))
                .ForMember(dest => dest.HoraSalida, opt => opt.MapFrom(src => src.HoraSalida.HasValue ? src.HoraSalida.Value.ToString(@"hh\:mm") : null));

            // CreateDTO -> Entity
            CreateMap<CreateAsistenciaDto, Asistencia>();

            // UpdateDTO -> Entity
            CreateMap<UpdateAsistenciaDto, Asistencia>()
                  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
