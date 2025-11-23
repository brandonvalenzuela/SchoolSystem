using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Materias;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly IMateriaService _service;

        public MateriasController(IMateriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<MateriaDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await _service.GetPagedAsync(page, size));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateMateriaDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMateriaDto dto)
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
