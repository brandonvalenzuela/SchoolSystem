using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Grados;
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
    public class GradoService : IGradoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GradoDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Grados.GetByIdAsync(id);
            return _mapper.Map<GradoDto>(entity);
        }

        public async Task<PagedResult<GradoDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Grados.GetAllIncludingAsync(
                g => g.NivelEducativo,
                g => g.Grupos,
                g => g.GradoMaterias
                );
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<GradoDto>
            {
                Items = _mapper.Map<IEnumerable<GradoDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateGradoDto dto)
        {
            var entity = _mapper.Map<Grado>(dto);
            await _unitOfWork.Grados.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateGradoDto dto)
        {
            var entity = await _unitOfWork.Grados.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Grado con ID {id} no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.Grados.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Grados.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Grado con ID {id} no encontrado");

            await _unitOfWork.Grados.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
