using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Comunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Application.Services.Interfaces
{
    public interface INotificacionService
    {
        Task<PagedResult<NotificacionDto>> GetMisNotificacionesAsync(int usuarioId, int pageNumber, int pageSize);
        Task<int> GetConteoNoLeidasAsync(int usuarioId);
        Task MarcarComoLeidaAsync(int notificacionId, int usuarioId);

        // Opcional: Crear notificación (si quisieras enviar desde código)
        // Task EnviarNotificacionAsync(...);
    }
}
