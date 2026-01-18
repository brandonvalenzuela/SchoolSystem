using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Academicos;
using SchoolSystem.Application.DTOs.Ciclos;
using SchoolSystem.Application.Services.Interfaces;
using System.Security.Claims;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CiclosController : ControllerBase
    {
        private readonly ICicloEscolarService _service;

        public CiclosController(ICicloEscolarService service)
        {
            _service = service;
        }

        [HttpGet("actual")]
        public async Task<ActionResult<ApiResponse<CicloEscolarActualDto>>> GetActual()
        {
            var escuelaIdStr = User.FindFirstValue("EscuelaId");
            if (!int.TryParse(escuelaIdStr, out var escuelaId))
                return BadRequest(new ApiResponse<CicloEscolarActualDto>("No se pudo obtener EscuelaId del token."));

            var dto = await _service.GetActualAsync(escuelaId);

            if (dto == null)
                return NotFound(new ApiResponse<CicloEscolarActualDto>("No existe un ciclo escolar actual configurado."));

            return Ok(new ApiResponse<CicloEscolarActualDto>(dto, "Ciclo escolar actual obtenido."));
        }
    }
}
