using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Grupos;
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
    public class GrupoService : IGrupoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GrupoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GrupoDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Grupos.GetByIdAsync(id);
            return _mapper.Map<GrupoDto>(entity);
        }

        public async Task<PagedResult<GrupoDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var allItems = await _unitOfWork.Grupos.GetAllAsync();
            var total = allItems.Count();
            var items = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<GrupoDto>
            {
                Items = _mapper.Map<IEnumerable<GrupoDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateGrupoDto dto)
        {
            var entity = _mapper.Map<Grupo>(dto);
            await _unitOfWork.Grupos.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateGrupoDto dto)
        {
            var entity = await _unitOfWork.Grupos.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Grupo con ID {id} no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.Grupos.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Grupos.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Grupo con ID {id} no encontrado");

            await _unitOfWork.Grupos.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
