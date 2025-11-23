using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EscuelasController : ControllerBase
    {
        private readonly IEscuelaService _service;

        public EscuelasController(IEscuelaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<EscuelaDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EscuelaDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateEscuelaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEscuelaDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
