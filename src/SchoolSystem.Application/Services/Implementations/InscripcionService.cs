using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Inscripciones;
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
    public class InscripcionService : IInscripcionService
    {
        private readonly IRepository<Inscripcion> _repository;
        private readonly IMapper _mapper;

        public InscripcionService(IRepository<Inscripcion> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InscripcionDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<InscripcionDto>(entity);
        }

        public async Task<PagedResult<InscripcionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _repository.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<InscripcionDto>
            {
                Items = _mapper.Map<IEnumerable<InscripcionDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateInscripcionDto dto)
        {
            var entity = _mapper.Map<Inscripcion>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateInscripcionDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Inscripción con ID {id} no encontrada");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Inscripción con ID {id} no encontrada");

            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
