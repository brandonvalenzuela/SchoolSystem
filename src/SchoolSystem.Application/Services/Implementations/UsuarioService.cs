using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Usuarios;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class    UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IRepository<Usuario> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<UsuarioDto>(entity);
        }

        public async Task<UsuarioDto> GetByUsernameAsync(string username)
        {
            // Asumiendo que tu repositorio genérico expone un método para buscar o acceder al DbSet
            var allUsers = await _repository.GetAllAsync();
            var user = allUsers.FirstOrDefault(u => u.Username == username);
            return _mapper.Map<UsuarioDto>(user);
        }

        public async Task<PagedResult<UsuarioDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var all = await _repository.GetAllAsync();
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
            entity.PasswordHash = dto.Password;

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
