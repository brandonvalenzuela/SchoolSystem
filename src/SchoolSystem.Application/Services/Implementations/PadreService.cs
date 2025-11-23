using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Academico;
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
        private readonly IRepository<Padre> _repository;
        private readonly IMapper _mapper;

        public PadreService(IRepository<Padre> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PadreDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<PadreDto>(entity);
        }

        public async Task<PagedResult<PadreDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _repository.GetAllAsync();
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
            var entity = _mapper.Map<Padre>(dto);
            // Nota: Aquí se asume que el mapeo maneja la creación del Usuario anidado
            // y el hasheo de contraseña si es necesario
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdatePadreDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Padre con ID {id} no encontrado");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Padre con ID {id} no encontrado");

            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
