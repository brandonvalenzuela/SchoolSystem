using AutoMapper;
using SchoolSystem.Application.DTOs.Maestros;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class MaestroProfile : Profile
    {
        public MaestroProfile()
        {
            // Entity -> DTO (Aplanamos los datos del Usuario dentro del MaestroDto)
            CreateMap<Maestro, MaestroDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario.NombreCompleto))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario.Telefono))
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Usuario.FotoUrl))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Usuario.Activo))
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus.ToString()));

            // CreateDTO -> Entity (Mapeo complejo hacia propiedades anidadas)
            CreateMap<CreateMaestroDto, Maestro>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => new Usuario
                {
                    EscuelaId = src.EscuelaId,
                    Username = src.Username,
                    Email = src.Email,
                    PasswordHash = src.Password, // Se hasheará en el servicio
                    Nombre = src.Nombre,
                    ApellidoPaterno = src.ApellidoPaterno,
                    ApellidoMaterno = src.ApellidoMaterno,
                    Telefono = src.Telefono,
                    Rol = SchoolSystem.Domain.Enums.Escuelas.RolUsuario.Maestro,
                    Activo = true
                }));

            // UpdateDTO -> Entity
            CreateMap<UpdateMaestroDto, Maestro>()
                .ForPath(dest => dest.Usuario.Username, opt => opt.MapFrom(src => src.Username))
                .ForPath(dest => dest.Usuario.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Usuario.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForPath(dest => dest.Usuario.ApellidoPaterno, opt => opt.MapFrom(src => src.ApellidoPaterno))
                .ForPath(dest => dest.Usuario.ApellidoMaterno, opt => opt.MapFrom(src => src.ApellidoMaterno))
                .ForPath(dest => dest.Usuario.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForPath(dest => dest.Usuario.Activo, opt => opt.MapFrom(src => src.Activo));
        }
    }
}
