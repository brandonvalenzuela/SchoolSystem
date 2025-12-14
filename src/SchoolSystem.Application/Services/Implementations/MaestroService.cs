using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Maestros;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
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
            // Repository.FindAsync returns IEnumerable<T> (lista).
            // Obtener el primer elemento (o null) antes de mapear a MaestroDto
            var entities = await _unitOfWork.Maestros.FindAsync(
                    c => c.Id == id,
                    c => c.Usuario,
                    c => c.Escuela
                );

            var entity = entities?.FirstOrDefault();

            if (entity == null) return null;

            return _mapper.Map<MaestroDto>(entity);
        }

        public async Task<PagedResult<MaestroDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var all = await _unitOfWork.Maestros.GetAllIncludingAsync(
                    m => m.Usuario,
                    c => c.Escuela
                );
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
            var maestros = await _unitOfWork.Maestros.GetAllIncludingAsync(
                    m => m.Usuario,
                    c => c.Escuela
                );

            var entity = maestros.FirstOrDefault(m => m.Id == id) ?? throw new KeyNotFoundException($"Maestro no encontrado");

            _mapper.Map(dto, entity);
            await _unitOfWork.Maestros.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var maestros = await _unitOfWork.Maestros.GetAllIncludingAsync(m => m.Usuario);
            var maestro = maestros.FirstOrDefault(m => m.Id == id) ?? throw new KeyNotFoundException($"Maestro no encontrado");

            // 2. Soft Delete del MAESTRO
            await _unitOfWork.Maestros.DeleteAsync(maestro);

            // Lógica de negocio adicional: Cambiar estatus
            maestro.Estatus = Domain.Enums.Academico.EstatusLaboral.Baja;
            maestro.FechaBaja = DateTime.Now;

            // 3. Soft Delete y Desactivación del USUARIO
            if (maestro.Usuario != null)
            {
                await _unitOfWork.Usuarios.DeleteAsync(maestro.Usuario);

                // Importante: Desactivar para que no pueda hacer Login
                maestro.Usuario.Activo = false;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
