using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Materias;
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
    public class MateriaService : IMateriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MateriaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MateriaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Materias.GetByIdAsync(id);
            return _mapper.Map<MateriaDto>(entity);
        }

        public async Task<PagedResult<MateriaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Materias.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<MateriaDto>
            {
                Items = _mapper.Map<IEnumerable<MateriaDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateMateriaDto dto)
        {
            var entity = _mapper.Map<Materia>(dto);
            await _unitOfWork.Materias.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateMateriaDto dto)
        {
            var entity = await _unitOfWork.Materias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Materia con ID {id} no encontrada");

            _mapper.Map(dto, entity);
            await _unitOfWork.Materias.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Materias.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Materia con ID {id} no encontrada");

            await _unitOfWork.Materias.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
