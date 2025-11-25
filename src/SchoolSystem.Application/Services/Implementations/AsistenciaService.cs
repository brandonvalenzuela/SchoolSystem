using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Asistencias;
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
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AsistenciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AsistenciaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            return _mapper.Map<AsistenciaDto>(entity);
        }

        public async Task<PagedResult<AsistenciaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Asistencias.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<AsistenciaDto>
            {
                Items = _mapper.Map<IEnumerable<AsistenciaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateAsistenciaDto dto)
        {
            var entity = _mapper.Map<Asistencia>(dto);
            await _unitOfWork.Asistencias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateAsistenciaDto dto)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Asistencia con ID {id} no encontrada");

            _mapper.Map(dto, entity);
            await _unitOfWork.Asistencias.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Asistencias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Asistencia con ID {id} no encontrada");

            await _unitOfWork.Asistencias.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
