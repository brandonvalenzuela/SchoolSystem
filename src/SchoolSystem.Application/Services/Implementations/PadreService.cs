using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums;
using SchoolSystem.Domain.Enums.Escuelas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class PadreService : IPadreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PadreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PadreDto> GetByIdAsync(int id)
        {
            var items = await _unitOfWork.Padres.GetAllIncludingAsync(
                p => p.Usuario,
                p => p.Escuela
                );

            var entity = items.FirstOrDefault(p => p.Id == id);

            if (entity == null)
                return null; // Manejo de no encontrado

            return _mapper.Map<PadreDto>(entity);
        }

        public async Task<PagedResult<PadreDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Padres.GetAllIncludingAsync(
                p => p.Usuario,
                p => p.Escuela
                );

            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<PadreDto>
            {
                Items = _mapper.Map<IEnumerable<PadreDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreatePadreDto dto)
        {
            // 1. Buscar si el usuario ya existe por email
            var usuarioExistente = await _unitOfWork.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            Padre padreFinal;

            if (usuarioExistente != null)
            {
                if (!usuarioExistente.EsPadre())
                    throw new InvalidOperationException("Este correo ya está registrado con un rol distinto.");

                // --- CAMBIO CLAVE AQUÍ ---
                // Buscamos si este usuario ya tiene un perfil de Padre en la tabla Padres
                var padresExistentes = await _unitOfWork.Padres.FindAsync(p => p.UsuarioId == usuarioExistente.Id && p.EscuelaId == dto.EscuelaId);
                padreFinal = padresExistentes.FirstOrDefault();

                // Si el usuario existe pero NO tiene perfil de padre (un caso raro pero posible), lo preparamos
                if (padreFinal == null)
                {
                    padreFinal = new Padre
                    {
                        EscuelaId = dto.EscuelaId,
                        UsuarioId = usuarioExistente.Id,
                        Ocupacion = dto.Ocupacion ?? "No especificada",
                        LugarTrabajo = dto.LugarTrabajo ?? "No especificado",
                        Observaciones = "Perfil creado para usuario existente"
                        // ... mapear los demás campos del DTO
                    };
                    await _unitOfWork.Padres.AddAsync(padreFinal);
                }
            }
            else
            {
                // 2. Crear nuevo Usuario y nuevo perfil de Padre (Flujo normal)
                var nuevoUsuario = new Usuario
                {
                    EscuelaId = dto.EscuelaId,
                    Email = dto.Email,
                    Username = dto.Email,
                    PasswordHash = Guid.NewGuid().ToString(),
                    Nombre = dto.Nombre,
                    ApellidoPaterno = dto.ApellidoPaterno,
                    ApellidoMaterno = dto.ApellidoMaterno,
                    Telefono = dto.Telefono,
                    FechaNacimiento = dto.FechaNacimiento ?? new DateTime(1980, 1, 1),
                    Genero = dto.Genero,
                    Rol = RolUsuario.Padre,
                    Activo = false
                };

                padreFinal = new Padre
                {
                    EscuelaId = dto.EscuelaId,
                    Usuario = nuevoUsuario,
                    Ocupacion = dto.Ocupacion ?? "No especificada",
                    LugarTrabajo = dto.LugarTrabajo ?? "No especificado",
                    Observaciones = dto.Observaciones ?? "Pre-registro administrativo",
                    Puesto = dto.Puesto,
                    Carrera = dto.Carrera,
                    NivelEstudios = dto.NivelEstudios,
                    DireccionTrabajo = dto.DireccionTrabajo,
                    EstadoCivil = dto.EstadoCivil,
                    AceptaSMS = dto.AceptaSMS,
                    AceptaEmail = dto.AceptaEmail,
                    AceptaPush = dto.AceptaPush
                };
                await _unitOfWork.Padres.AddAsync(padreFinal);
            }

            // 3. Crear el vínculo con el Alumno (AlumnoPadre)
            if (dto.AlumnoId > 0)
            {
                // Validar que no estén ya vinculados para evitar otro error de duplicados
                var relacionExistente = (await _unitOfWork.AlumnoPadres.FindAsync(ap =>
                    ap.AlumnoId == dto.AlumnoId && ap.PadreId == padreFinal.Id));

                if (relacionExistente.Any())
                {
                    throw new InvalidOperationException(
                        $"El tutor {padreFinal.Usuario.NombreCompleto} ya está vinculado a este alumno.");
                }

                var nuevaRelacion = new AlumnoPadre
                {
                    AlumnoId = dto.AlumnoId,
                    Padre = padreFinal, // EF se encarga de usar el ID correcto ya sea nuevo o existente
                    Relacion = Enum.TryParse<RelacionFamiliar>(dto.Relacion, out var resultado) ? resultado : RelacionFamiliar.Tutor,
                    RecibeNotificaciones = true,
                    EsTutorPrincipal = true,
                    ViveConAlumno = true,
                    AutorizadoRecoger = true
                };

                await _unitOfWork.AlumnoPadres.AddAsync(nuevaRelacion);
            }

            // 4. Guardar cambios
            await _unitOfWork.SaveChangesAsync();

            return padreFinal.Id;
        }

        public async Task UpdateAsync(int id, UpdatePadreDto dto)
        {
            var padres = await _unitOfWork.Padres.GetAllIncludingAsync(p => p.Usuario);

            var entity = padres.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException($"Padre {id} no encontrado");

            // Aseguramos que el usuario asociado esté cargado para que AutoMapper no falle con el PasswordHash
            if (entity.Usuario == null)
            {
                // Esto no debería pasar si la integridad referencial está bien, pero es una protección
                throw new InvalidOperationException("El registro del padre no tiene un usuario asociado válido.");
            }

            _mapper.Map(dto, entity);
            await _unitOfWork.Padres.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Usamos la lógica de Soft Delete que definimos para Maestros
            var padres = await _unitOfWork.Padres.GetAllIncludingAsync(p => p.Usuario);
            var entity = padres.FirstOrDefault(p => p.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"Padre con ID {id} no encontrado");

            // Borramos la entidad hija (Padre)
            // Si tienes SoftDelete implementado en Padre, esto marcará IsDeleted = true
            // Si no, hará hard delete. 
            // Para mantener consistencia con Maestros, deberías implementar ISoftDeletable en Padre también.
            await _unitOfWork.Padres.DeleteAsync(entity);

            // Opcional: Desactivar al usuario base
            if (entity.Usuario != null)
            {
                entity.Usuario.Activo = false;
                // entity.Usuario.IsDeleted = true; // Si implementaste soft delete en usuario
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
