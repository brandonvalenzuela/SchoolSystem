using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Maestros;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaestrosController : ControllerBase
    {
        private readonly IMaestroService _service;

        public MaestrosController(IMaestroService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de maestros.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de MaestroDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<MaestroDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<MaestroDto>>(result, "Listado de maestros obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un maestro por su ID.
        /// </summary>
        /// <param name="id">ID del maestro</param>
        /// <returns>MaestroDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MaestroDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<MaestroDto>("Maestro no encontrado."));

            return Ok(new ApiResponse<MaestroDto>(result, "Maestro encontrado exitosamente."));
        }

        /// <summary>
        /// Registra un nuevo maestro.
        /// </summary>
        /// <param name="dto">Datos del maestro</param>
        /// <returns>ID del maestro creado envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateMaestroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos del maestro inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Maestro registrado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un maestro existente.
        /// </summary>
        /// <param name="id">ID del maestro a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateMaestroDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Maestro actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un maestro.
        /// </summary>
        /// <param name="id">ID del maestro</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<bool>(true, "Maestro eliminado exitosamente."));
        }
    }
}