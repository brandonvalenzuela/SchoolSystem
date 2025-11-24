using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Escuelas;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

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

        /// <summary>
        /// Obtiene una lista paginada de escuelas.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de EscuelaDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<EscuelaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<EscuelaDto>>(result, "Listado de escuelas obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una escuela por su ID.
        /// </summary>
        /// <param name="id">ID de la escuela</param>
        /// <returns>EscuelaDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EscuelaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<EscuelaDto>("Escuela no encontrada."));

            return Ok(new ApiResponse<EscuelaDto>(result, "Escuela encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva escuela.
        /// </summary>
        /// <param name="dto">Datos de la escuela</param>
        /// <returns>ID de la escuela creada envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateEscuelaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de la escuela inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Escuela registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una escuela existente.
        /// </summary>
        /// <param name="id">ID de la escuela a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateEscuelaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Escuela actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una escuela.
        /// </summary>
        /// <param name="id">ID de la escuela</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Escuela eliminada exitosamente."));
        }
    }
}