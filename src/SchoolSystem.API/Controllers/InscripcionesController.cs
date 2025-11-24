using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Inscripciones;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionService _service;

        public InscripcionesController(IInscripcionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de inscripciones.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de InscripcionDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<InscripcionDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<InscripcionDto>>(result, "Listado de inscripciones obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una inscripción por su ID.
        /// </summary>
        /// <param name="id">ID de la inscripción</param>
        /// <returns>InscripcionDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<InscripcionDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<InscripcionDto>("Inscripción no encontrada."));

            return Ok(new ApiResponse<InscripcionDto>(result, "Inscripción encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva inscripción (inscribe a un alumno en un grupo).
        /// </summary>
        /// <param name="dto">Datos de la inscripción</param>
        /// <returns>ID de la inscripción creada envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateInscripcionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de inscripción inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Inscripción registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una inscripción existente (ej. cambio de grupo o baja).
        /// </summary>
        /// <param name="id">ID de la inscripción a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateInscripcionDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Inscripción actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una inscripción.
        /// </summary>
        /// <param name="id">ID de la inscripción</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Inscripción eliminada exitosamente."));
        }
    }
}