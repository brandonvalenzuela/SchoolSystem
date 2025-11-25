using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Asistencias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsistenciasController : ControllerBase
    {
        private readonly IAsistenciaService _service;

        public AsistenciasController(IAsistenciaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de registros de asistencia.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de asistencias envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AsistenciaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<AsistenciaDto>>(result, "Listado de asistencias obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un registro de asistencia por su ID.
        /// </summary>
        /// <param name="id">ID de la asistencia</param>
        /// <returns>AsistenciaDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AsistenciaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<AsistenciaDto>("Registro de asistencia no encontrado."));

            return Ok(new ApiResponse<AsistenciaDto>(result, "Asistencia encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva asistencia.
        /// </summary>
        /// <param name="dto">Datos de la asistencia</param>
        /// <returns>ID de la asistencia creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAsistenciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de asistencia inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Asistencia registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un registro de asistencia existente (ej. para justificar una falta).
        /// </summary>
        /// <param name="id">ID de la asistencia a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateAsistenciaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            // Se usa Ok en lugar de NoContent para devolver el mensaje de éxito
            return Ok(new ApiResponse<int>(id, "Registro de asistencia actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un registro de asistencia.
        /// </summary>
        /// <param name="id">ID de la asistencia</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Registro de asistencia eliminado exitosamente."));
        }
    }
}