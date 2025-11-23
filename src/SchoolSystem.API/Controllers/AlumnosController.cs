using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnoService _service;

        public AlumnosController(IAlumnoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de alumnos.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de AlumnoDto</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResult<AlumnoDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un alumno por su ID.
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>AlumnoDto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo alumno.
        /// </summary>
        /// <param name="dto">Datos del alumno</param>
        /// <returns>ID del alumno creado</returns>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateAlumnoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Actualiza un alumno existente.
        /// </summary>
        /// <param name="id">ID del alumno a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAlumnoDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Elimina (lógicamente) un alumno.
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
