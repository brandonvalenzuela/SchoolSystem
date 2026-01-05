using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Medico;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpedientesController : ControllerBase
    {
        private readonly IMedicoService _service;

        public ExpedientesController(IMedicoService service)
        {
            _service = service;
        }

        [HttpGet("alumno/{alumnoId}")]
        [Authorize(Roles = Roles.Staff + "," + Roles.Padre)]
        public async Task<ActionResult<ApiResponse<ExpedienteMedicoDto>>> GetByAlumno(int alumnoId)
        {
            var result = await _service.GetByAlumnoIdAsync(alumnoId);
            if (result == null)
                return NotFound(new ApiResponse<ExpedienteMedicoDto>("Expediente no encontrado."));
            return Ok(new ApiResponse<ExpedienteMedicoDto>(result, "Expediente encontrado."));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateExpedienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));
            var id = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<int>(id, "Expediente creado."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateExpedienteDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("ID mismatch."));
            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<int>(id, "Expediente actualizado."));
        }
    }
}
