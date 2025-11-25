using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Calificaciones;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _service;

        public CalificacionesController(ICalificacionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de calificaciones.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de CalificacionDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CalificacionDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<CalificacionDto>>(result, "Listado de calificaciones obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una calificación por su ID.
        /// </summary>
        /// <param name="id">ID de la calificación</param>
        /// <returns>CalificacionDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CalificacionDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<CalificacionDto>("Calificación no encontrada."));

            return Ok(new ApiResponse<CalificacionDto>(result, "Calificación encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva calificación.
        /// </summary>
        /// <param name="dto">Datos de la calificación</param>
        /// <returns>ID de la calificación creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateCalificacionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de calificación inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Calificación registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una calificación existente.
        /// </summary>
        /// <param name="id">ID de la calificación a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateCalificacionDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Calificación actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una calificación.
        /// </summary>
        /// <param name="id">ID de la calificación</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Calificación eliminada exitosamente."));
        }
    }
}