using AutoMapper;
using SchoolSystem.Application.DTOs.Maestros; // Asegúrate que este namespace sea correcto según donde guardaste los DTOs
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Escuelas; // Para RolUsuario

namespace SchoolSystem.Application.Mappings
{
    public class MaestroProfile : Profile
    {
        public MaestroProfile()
        {
            // ---------------------------------------------------------
            // 1. Lectura: Entity -> DTO
            // ---------------------------------------------------------
            CreateMap<Maestro, MaestroDto>()
                // Datos calculados o directos
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario.NombreCompleto))
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus.ToString()))

                // Datos del Usuario (Mapeo explícito necesario para el formulario de edición)
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Usuario.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.ApellidoPaterno, opt => opt.MapFrom(src => src.Usuario.ApellidoPaterno))
                .ForMember(dest => dest.ApellidoMaterno, opt => opt.MapFrom(src => src.Usuario.ApellidoMaterno))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario.Telefono))
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Usuario.FotoUrl))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Usuario.Activo));

            // ---------------------------------------------------------
            // 2. Creación: DTO -> Entity
            // ---------------------------------------------------------
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
                    Rol = RolUsuario.Maestro,
                    Activo = true,
                    CreatedAt = DateTime.UtcNow // Buena práctica inicializar
                }));

            // ---------------------------------------------------------
            // 3. Actualización: DTO -> Entity
            // ---------------------------------------------------------
            CreateMap<UpdateMaestroDto, Maestro>()
                // Usamos ForPath para actualizar propiedades anidadas en el objeto Usuario
                .ForPath(dest => dest.Usuario.Username, opt => opt.MapFrom(src => src.Username))
                .ForPath(dest => dest.Usuario.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Usuario.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForPath(dest => dest.Usuario.ApellidoPaterno, opt => opt.MapFrom(src => src.ApellidoPaterno))
                .ForPath(dest => dest.Usuario.ApellidoMaterno, opt => opt.MapFrom(src => src.ApellidoMaterno))
                .ForPath(dest => dest.Usuario.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForPath(dest => dest.Usuario.Activo, opt => opt.MapFrom(src => src.Activo))

                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}