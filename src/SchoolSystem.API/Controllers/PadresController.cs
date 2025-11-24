using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Padres;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

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

        /// <summary>
        /// Obtiene una lista paginada de padres de familia.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de PadreDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PadreDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<PadreDto>>(result, "Listado de padres obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un padre de familia por su ID.
        /// </summary>
        /// <param name="id">ID del padre</param>
        /// <returns>PadreDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PadreDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<PadreDto>("Padre no encontrado."));

            return Ok(new ApiResponse<PadreDto>(result, "Padre encontrado exitosamente."));
        }

        /// <summary>
        /// Registra un nuevo padre de familia.
        /// </summary>
        /// <param name="dto">Datos del padre</param>
        /// <returns>ID del padre creado envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreatePadreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos del padre inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Padre registrado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un padre de familia existente.
        /// </summary>
        /// <param name="id">ID del padre a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdatePadreDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Padre actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un padre de familia.
        /// </summary>
        /// <param name="id">ID del padre</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Padre eliminado exitosamente."));
        }
    }
}