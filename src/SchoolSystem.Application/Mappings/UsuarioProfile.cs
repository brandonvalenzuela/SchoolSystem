using AutoMapper;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Entity -> DTO (Lectura)
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Rol,
                           opt => opt.MapFrom(src => src.Rol.ToString()))
                .ForMember(dest => dest.Genero,
                           opt => opt.MapFrom(src => src.Genero.ToString()));

            // CreateDTO -> Entity
            CreateMap<CreateUsuarioDto, Usuario>()
                // Nota: La contraseña se debe hashear en el servicio,
                // pero aquí mapeamos el string plano inicialmente.
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            // UpdateDTO -> Entity
            CreateMap<UpdateUsuarioDto, Usuario>();
        }
    }
}
