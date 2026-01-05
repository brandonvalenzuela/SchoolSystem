using AutoMapper;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Comunicacion;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Implementations
{
    public class NotificacionService : INotificacionService
    {
        private readonly IUnitOfWork _unitOfWork; // Asumiendo que agregaste el repo de notificaciones aquí
        private readonly IMapper _mapper;

        public NotificacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<NotificacionDto>> GetMisNotificacionesAsync(int usuarioId, int pageNumber, int pageSize)
        {
            // Necesitamos acceder al repositorio de notificaciones.
            // Si no lo agregaste al UnitOfWork, hazlo (IRepository<Notificacion> Notificaciones { get; })

            // Filtro: Solo del usuario, activas y no borradas
            var query = await _unitOfWork.Notificaciones.FindAsync(n =>
                n.UsuarioDestinatarioId == usuarioId &&
                !n.IsDeleted);

            var total = query.Count();

            var items = query
                .OrderByDescending(n => n.FechaEnvio)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<NotificacionDto>
            {
                Items = _mapper.Map<IEnumerable<NotificacionDto>>(items),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> GetConteoNoLeidasAsync(int usuarioId)
        {
            var noLeidas = await _unitOfWork.Notificaciones.FindAsync(n =>
                n.UsuarioDestinatarioId == usuarioId &&
                !n.Leida &&
                !n.IsDeleted);

            return noLeidas.Count();
        }

        public async Task MarcarComoLeidaAsync(int notificacionId, int usuarioId)
        {
            var notificacion = await _unitOfWork.Notificaciones.GetByIdAsync(notificacionId);

            if (notificacion == null)
                throw new KeyNotFoundException("Notificación no encontrada.");

            if (notificacion.UsuarioDestinatarioId != usuarioId)
                throw new UnauthorizedAccessException("Esta notificación no te pertenece.");

            if (!notificacion.Leida)
            {
                notificacion.Leida = true;
                notificacion.FechaLectura = DateTime.Now;

                await _unitOfWork.Notificaciones.UpdateAsync(notificacion);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
