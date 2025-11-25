using AutoMapper;
using FluentValidation.Validators;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Conducta;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Entities.Conducta;
using SchoolSystem.Domain.Enums.Conducta;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class RegistroConductaService : IRegistroConductaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegistroConductaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RegistroConductaDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.RegistroConductas.GetByIdAsync(id);

            if (entity == null) return null;

            return _mapper.Map<RegistroConductaDto>(entity);
        }

        public async Task<PagedResult<RegistroConductaDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            // USAMOS EL NUEVO MÉTODO CON INCLUDES
            // Nota: Para 'Maestro.Usuario', EF Core es inteligente, 
            // pero si falla, a veces se requiere .ThenInclude (lo cual requiere otro método en el repo)
            // o simplemente incluir "Maestro" y "Maestro.Usuario" si usas strings.
            // Con expresiones simples:
            var allItems = await _unitOfWork.RegistroConductas.GetAllIncludingAsync(
                rc => rc.Alumno,            // Para obtener NombreCompleto del alumno
                rc => rc.Maestro,           // Relación Maestro
                rc => rc.Maestro.Usuario    // Para obtener NombreCompleto del maestro (que es un Usuario)
            );

            // Ordenamiento (en memoria, ya que trajimos la lista)
            var orderedItems = allItems.OrderByDescending(r => r.FechaHoraIncidente);

            var total = orderedItems.Count();

            // Paginación
            var items = orderedItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Mapeo a DTO
            var dtos = _mapper.Map<IEnumerable<RegistroConductaDto>>(items);

            return new PagedResult<RegistroConductaDto>
            {
                Items = dtos,
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateRegistroConductaDto dto)
        {
            var entity = _mapper.Map<RegistroConducta>(dto);

            // Lógica de negocio por defecto
            if (entity.FechaHoraIncidente == default)
            {
                entity.FechaHoraIncidente = DateTime.Now;
            }

            // Establecer estado inicial
            entity.Estado = EstadoRegistroConducta.Activo;
            entity.MetodoNotificacion = TipoNotificacion.Ninguna;

            // Calcular impacto automático si es necesario (Opcional, depende de tu lógica)
            entity.Puntos = entity.CalcularPuntosImpacto();
            // Por ahora confiamos en lo que envía el DTO o el frontend.

            await _unitOfWork.RegistroConductas.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateRegistroConductaDto dto)
        {
            var entity = await _unitOfWork.RegistroConductas.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registro de conducta #{id} no encontrado.");

            // AutoMapper actualiza solo los campos que vienen en el DTO
            _mapper.Map(dto, entity);

            await _unitOfWork.RegistroConductas.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.RegistroConductas.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registro de conducta #{id} no encontrado.");

            // Soft Delete (si el repositorio lo soporta) o Hard Delete
            await _unitOfWork.RegistroConductas.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
