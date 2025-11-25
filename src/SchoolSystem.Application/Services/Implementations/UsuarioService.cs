using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Usuarios;
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

            // IMPORTANTE: Aquí deberías hashear la contraseña
            // entity.PasswordHash = _passwordHasher.Hash(dto.Password);
            // Por ahora, mapeo simple para el ejemplo (NO HACER EN PRODUCCIÓN SIN HASH)
            entity.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            await _unitOfWork.Usuarios.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            var entity = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.Usuarios.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            await _unitOfWork.Usuarios.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
