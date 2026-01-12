using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Academicos;
using SchoolSystem.Application.Services.Implementations;
using SchoolSystem.Domain.Constants;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RelacionesController : Controller
    {
        private readonly IRelacionService _service;

        public RelacionesController(IRelacionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Vincular([FromBody] CreateAlumnoPadreDto dto)
        {
            var id = await _service.VincularAsync(dto);
            return Ok(new ApiResponse<int>(id, "Vínculo creado."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> Desvincular(int id)
        {
            await _service.DesvincularAsync(id);
            return Ok(new ApiResponse<bool>(true, "Vínculo eliminado."));
        }

        [HttpGet("padre/{padreId}")]
        public async Task<ActionResult<ApiResponse<List<AlumnoPadreDto>>>> GetPorPadre(int padreId)
        {
            var lista = await _service.GetPorPadreAsync(padreId);
            return Ok(new ApiResponse<List<AlumnoPadreDto>>(lista, "Hijos obtenidos."));
        }

        [HttpGet("alumno/{alumnoId}")]
        public async Task<ActionResult<ApiResponse<List<AlumnoPadreDto>>>> GetPorAlumno(int alumnoId)
        {
            var lista = await _service.GetPorAlumnoAsync(alumnoId);
            return Ok(new ApiResponse<List<AlumnoPadreDto>>(lista, "Padres obtenidos."));
        }
    }
}
