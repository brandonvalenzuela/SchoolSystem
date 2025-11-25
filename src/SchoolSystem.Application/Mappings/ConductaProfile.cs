using AutoMapper;
using SchoolSystem.Application.DTOs.Conducta;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Enums.Conducta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class ConductaProfile : Profile
    {
        public ConductaProfile()
        {
            // 1. De Entidad a DTO de Lectura
            CreateMap<RegistroConducta, RegistroConductaDto>()
                // Mapear nombres de relaciones (manejar posibles nulos)
                .ForMember(dest => dest.NombreAlumno,
                    opt => opt.MapFrom(src => src.Alumno != null ? src.Alumno.NombreCompleto : "Desconocido"))
                .ForMember(dest => dest.NombreMaestro,
                    opt => opt.MapFrom(src => src.Maestro != null && src.Maestro.Usuario != null ? src.Maestro.Usuario.NombreCompleto : "Desconocido"))
                // Mapear Enums a String para lectura fácil
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.ToString()))
                .ForMember(dest => dest.Gravedad, opt => opt.MapFrom(src => src.Gravedad.HasValue ? src.Gravedad.ToString() : "N/A"))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));

            // 2. De DTO Creación a Entidad
            CreateMap<CreateRegistroConductaDto, RegistroConducta>()
                // Si la fecha viene nula, se asignará en el servicio, pero aquí mapeamos directo
                .ForMember(dest => dest.FechaHoraIncidente, opt => opt.Condition(src => src.FechaHoraIncidente.HasValue));

            // 3. De DTO Actualización a Entidad
            CreateMap<UpdateRegistroConductaDto, RegistroConducta>()
                // Ignorar nulos para no sobrescribir datos existentes con vacíos
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
