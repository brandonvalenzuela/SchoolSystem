using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Comunicacion;
using SchoolSystem.Application.Services.Interfaces;
using System.Security.Claims;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacionesController : Controller
    {
        private readonly INotificacionService _service;

        public NotificacionesController(INotificacionService service)
        {
            _service = service;
        }

        [HttpGet("mis-notificaciones")]
        public async Task<ActionResult<ApiResponse<PagedResult<NotificacionDto>>>> GetMisNotificaciones([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var userId = GetCurrentUserId();
            var result = await _service.GetMisNotificacionesAsync(userId, page, size);
            return Ok(new ApiResponse<PagedResult<NotificacionDto>>(result, "Notificaciones obtenidas."));
        }

        [HttpGet("conteo-noleidas")]
        public async Task<ActionResult<ApiResponse<int>>> GetConteo()
        {
            var userId = GetCurrentUserId();
            var count = await _service.GetConteoNoLeidasAsync(userId);
            return Ok(new ApiResponse<int>(count, "Conteo obtenido."));
        }

        [HttpPut("{id}/leer")]
        public async Task<ActionResult<ApiResponse<bool>>> MarcarComoLeida(int id)
        {
            var userId = GetCurrentUserId();
            await _service.MarcarComoLeidaAsync(id, userId);
            return Ok(new ApiResponse<bool>(true, "Notificación marcada como leída."));
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return idClaim != null && int.TryParse(idClaim.Value, out int id) ? id : 0;
        }
    }
}
