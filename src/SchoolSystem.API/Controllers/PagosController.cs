using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Finanzas;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;

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
                var id = await _service.RegistrarPagoAsync(dto);
                return Ok(new ApiResponse<int>(id, "Pago registrado correctamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>(ex.Message));
            }
        }

        // Endpoint para cancelar pago...
    }

}
