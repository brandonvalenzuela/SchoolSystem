using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PadresController : ControllerBase
    {
        private readonly IPadreService _service;

        public PadresController(IPadreService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PadreDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await _service.GetPagedAsync(page, size));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PadreDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreatePadreDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePadreDto dto)
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
