using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Finanzas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Security.Claims;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagosController : ControllerBase
    {
        private readonly IPagoService _service;

        public PagosController(IPagoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<PagedResult<PagoDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagosAsync(page, size);
            return Ok(new ApiResponse<PagedResult<PagoDto>>(result, "Pagos obtenidos."));
        }

        [HttpGet("pendientes/alumno/{alumnoId}")]
        public async Task<ActionResult<ApiResponse<List<CargoDto>>>> GetPendientes(int alumnoId)
        {
            var result = await _service.GetCargosPendientesPorAlumnoAsync(alumnoId);
            return Ok(new ApiResponse<List<CargoDto>>(result, "Cargos pendientes obtenidos."));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)] // Solo administradores cobran
        public async Task<ActionResult<ApiResponse<int>>> RegistrarPago([FromBody] CreatePagoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ApiResponse<int>("No se pudo identificar al usuario que realiza el cobro."));
                }

                dto.RecibidoPorId = userId;

                var id = await _service.RegistrarPagoAsync(dto);
                return Ok(new ApiResponse<int>(id, "Pago registrado correctamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>(ex.Message));
            }
        }

        [HttpPost("{id}/cancelar")]
        [Authorize(Roles = Roles.Admin)] // Solo Directores/SuperAdmin deberían cancelar pagos
        public async Task<ActionResult<ApiResponse<bool>>> CancelarPago(int id, [FromBody] CancelarPagoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<bool>("Datos inválidos."));

            try
            {
                // Obtener ID del usuario actual
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                int userId = int.Parse(userIdClaim?.Value ?? "0");

                await _service.CancelarPagoAsync(id, dto.Motivo, userId);
                return Ok(new ApiResponse<bool>(true, "Pago cancelado exitosamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>(ex.Message));
            }
        }
    }

}
