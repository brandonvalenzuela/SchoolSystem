using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _service;

        public CalificacionesController(ICalificacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CalificacionDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await _service.GetPagedAsync(page, size));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalificacionDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCalificacionDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCalificacionDto dto)
        {
            if (id != dto.Id)
                return BadRequest();
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
