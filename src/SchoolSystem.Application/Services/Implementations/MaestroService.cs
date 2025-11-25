using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Maestros;
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
    public class MaestroService : IMaestroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaestroService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MaestroDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Maestros.GetByIdAsync(id);
            return _mapper.Map<MaestroDto>(entity);
        }

        public async Task<PagedResult<MaestroDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var all = await _unitOfWork.Maestros.GetAllAsync();
            var total = all.Count();
            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedResult<MaestroDto>
            {
                Items = _mapper.Map<IEnumerable<MaestroDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateMaestroDto dto)
        {
            // AutoMapper debe estar configurado para mapear CreateMaestroDto -> Maestro
            // incluyendo la creación del objeto Usuario anidado si es necesario
            var entity = _mapper.Map<Maestro>(dto);

            // Simulación de hash de password para el usuario interno
            if (entity.Usuario != null)
                entity.Usuario.PasswordHash = dto.Password;

            await _unitOfWork.Maestros.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateMaestroDto dto)
        {
            var entity = await _unitOfWork.Maestros.GetByIdAsync(id);
            if (entity == null)
                throw new System.Collections.Generic.KeyNotFoundException($"Maestro no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.Maestros.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Maestros.GetByIdAsync(id);
            if (entity == null)
                throw new System.Collections.Generic.KeyNotFoundException($"Maestro no encontrado");

            await _unitOfWork.Maestros.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
