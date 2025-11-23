using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<UsuarioDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");
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