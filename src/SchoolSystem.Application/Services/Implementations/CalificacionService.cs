using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Evaluacion;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class CalificacionService : ICalificacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CalificacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CalificacionDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id);
            return _mapper.Map<CalificacionDto>(entity);
        }

        public async Task<PagedResult<CalificacionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Calificaciones.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<CalificacionDto>
            {
                Items = _mapper.Map<IEnumerable<CalificacionDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateCalificacionDto dto)
        {
            var entity = _mapper.Map<Calificacion>(dto);
            await _unitOfWork.Calificaciones.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateCalificacionDto dto)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            _mapper.Map(dto, entity);
            await _unitOfWork.Calificaciones.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Calificaciones.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Calificación con ID {id} no encontrada");

            await _unitOfWork.Calificaciones.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
