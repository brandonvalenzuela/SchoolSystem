using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Enums.Escuelas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class    UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<UsuarioDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Usuarios.GetByIdAsync(id);
            return _mapper.Map<UsuarioDto>(entity);
        }

        public async Task<UsuarioDto> GetByUsernameAsync(string username)
        {
            // Asumiendo que tu repositorio genérico expone un método para buscar o acceder al DbSet
            var allUsers = await _unitOfWork.Usuarios.GetAllAsync();
            var user = allUsers.FirstOrDefault(u => u.Username == username);
            return _mapper.Map<UsuarioDto>(user);
        }

        public async Task<PagedResult<UsuarioDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var all = await _unitOfWork.Usuarios.GetAllAsync();
            var total = all.Count();
            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<UsuarioDto>
            {
                Items = _mapper.Map<IEnumerable<UsuarioDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateUsuarioDto dto)
        {
            var entity = _mapper.Map<Usuario>(dto);

            entity.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            await _unitOfWork.Usuarios.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            // REGLA DE NEGOCIO: Protección de Roles Especializados
            // Si intentan cambiar el rol y el usuario ya es de un tipo especial, debemos validar
            if (usuario.Rol != dto.Rol)
            {
                if (EsRolEspecializado(usuario.Rol))
                {
                    // Aquí podrías ser más sofisticado y verificar si realmente existe el registro en la otra tabla,
                    // pero por seguridad, es mejor prohibir el cambio de rol directo.
                    throw new InvalidOperationException(
                        $"No se puede cambiar el rol de un usuario tipo '{usuario.Rol}' directamente. " +
                        "Debe dar de baja el perfil especializado primero.");
                }
            }
            // REGLA: No permitir desactivar al propio SuperAdmin actual (opcional, requiere contexto de usuario actual)
            if (usuario.Rol == RolUsuario.SuperAdmin && !dto.Activo)
            {     
                throw new InvalidOperationException("No se puede desactivar al SuperAdmin actual."); 
            }

            _mapper.Map(dto, usuario);

            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            // REGLA 1: Protección de SuperAdmin
            if (usuario.Rol == RolUsuario.SuperAdmin)
            {
                throw new InvalidOperationException("No se puede eliminar a un Super Administrador.");
            }

            // REGLA 2: Redirección de responsabilidad para Roles Especializados
            if (EsRolEspecializado(usuario.Rol))
            {
                string modulo = ObtenerNombreModulo(usuario.Rol);
                throw new InvalidOperationException(
                    $"Este usuario tiene el rol '{usuario.Rol}'. " +
                    $"Para mantener la integridad de los datos, debe eliminarlo desde el módulo de **{modulo}**.");
            }

            await _unitOfWork.Usuarios.DeleteAsync(usuario);

            await _unitOfWork.SaveChangesAsync();
        }

        private bool EsRolEspecializado(RolUsuario rol)
        {
            return rol == RolUsuario.Maestro ||
                   rol == RolUsuario.Padre ||
                   rol == RolUsuario.Alumno;
        }

        private string ObtenerNombreModulo(RolUsuario rol)
        {
            return rol switch
            {
                RolUsuario.Maestro => "Maestros",
                RolUsuario.Padre => "Padres de Familia",
                RolUsuario.Alumno => "Alumnos",
                _ => "General"
            };
        }

        public async Task SolicitarActivacionAppAsync(string email)
        {
            var usuario = await _unitOfWork.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            // Solo permitimos activar si el usuario existe y no ha sido activado aún
            if (usuario == null || usuario.Activo)
                throw new InvalidOperationException("El correo no es válido para activación.");

            // 1. Generamos un código de 6 dígitos
            string codigo = new Random().Next(100000, 999999).ToString();

            // 2. Reusamos tus campos existentes
            usuario.TokenRecuperacion = codigo;
            usuario.TokenExpiracion = DateTime.UtcNow.AddMinutes(15); // Tiempo corto para seguridad

            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();

            // 3. Enviar por email (Opcional por ahora)
            // _emailService.Send(email, "Tu código de activación es: " + codigo);
        }

        public async Task CompletarActivacionAppAsync(string email, string codigo, string newUsername, string newPassword)
        {
            var usuario = await _unitOfWork.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            // Validar el token reusado
            if (usuario == null || usuario.TokenRecuperacion != codigo || usuario.TokenExpiracion < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Código inválido o expirado.");
            }

            // 4. Activación final
            usuario.Username = newUsername;
            usuario.PasswordHash = _passwordHasher.HashPassword(newPassword); // Ya hasheado
            usuario.Activo = true;

            // Limpiar el token para que no se pueda reusar
            usuario.TokenRecuperacion = null;
            usuario.TokenExpiracion = null;

            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
