using AutoMapper;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Escuelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class PadreProfile : Profile
    {
        public PadreProfile()
        {
            // Entity -> DTO
            CreateMap<Padre, PadreDto>()
                // Datos calculados o directos
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario.NombreCompleto))

                // --- MAPEOS EXPLÍCITOS QUE FALTABAN ---
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Usuario.Username))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.ApellidoPaterno, opt => opt.MapFrom(src => src.Usuario.ApellidoPaterno))
                .ForMember(dest => dest.ApellidoMaterno, opt => opt.MapFrom(src => src.Usuario.ApellidoMaterno))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario.Telefono))
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Usuario.FotoUrl))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Usuario.Activo))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.Usuario.FechaNacimiento))
                .ForMember(dest => dest.Ocupacion, opt => opt.MapFrom(src => src.Ocupacion));


            // CreateDTO -> Entity
            CreateMap<CreatePadreDto, Padre>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => new Usuario
                {
                    EscuelaId = src.EscuelaId,
                    Email = src.Email,
                    Nombre = src.Nombre,
                    ApellidoPaterno = src.ApellidoPaterno,
                    ApellidoMaterno = src.ApellidoMaterno,
                    FechaNacimiento = src.FechaNacimiento,
                    Telefono = src.Telefono,
                    Rol = RolUsuario.Padre,
                    Activo = true,
                    CreatedAt = DateTime.UtcNow
                }));

            // UpdateDTO -> Entity
            CreateMap<UpdatePadreDto, Padre>()
                .ForPath(dest => dest.Usuario.Username, opt => opt.MapFrom(src => src.Username))
                .ForPath(dest => dest.Usuario.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Usuario.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForPath(dest => dest.Usuario.ApellidoPaterno, opt => opt.MapFrom(src => src.ApellidoPaterno))
                .ForPath(dest => dest.Usuario.ApellidoMaterno, opt => opt.MapFrom(src => src.ApellidoMaterno))
                .ForPath(dest => dest.Usuario.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForPath(dest => dest.Usuario.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForPath(dest => dest.Usuario.Activo, opt => opt.MapFrom(src => src.Activo))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
