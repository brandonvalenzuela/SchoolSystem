using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Escuelas;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class EscuelaService : IEscuelaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EscuelaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EscuelaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            return _mapper.Map<EscuelaDto>(entity);
        }

        public async Task<PagedResult<EscuelaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Escuelas.GetAllAsync();

            var total = allItems.Count();
            var items = allItems
                .OrderBy(e => e.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<EscuelaDto>
            {
                Items = _mapper.Map<IEnumerable<EscuelaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateEscuelaDto dto)
        {
            var entity = _mapper.Map<Escuela>(dto);

            // --- CORRECCIÓN AQUÍ ---
            // Como el DTO no tiene 'Activo', establecemos valores por defecto del sistema.
            entity.FechaRegistro = DateTime.Now;
            entity.Activo = true; // Por defecto, una escuela nueva nace activa.

            await _unitOfWork.Escuelas.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateEscuelaDto dto)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Escuela con ID {id} no encontrada");

            _mapper.Map(dto, entity);

            await _unitOfWork.Escuelas.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Escuelas.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Escuela con ID {id} no encontrada");

            await _unitOfWork.Escuelas.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
