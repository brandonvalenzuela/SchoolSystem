using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Grupos;
using SchoolSystem.Application.Services.Interfaces;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoService _service;

        public GruposController(IGrupoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de grupos escolares.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de GrupoDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<GrupoDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<GrupoDto>>(result, "Listado de grupos obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un grupo por su ID.
        /// </summary>
        /// <param name="id">ID del grupo</param>
        /// <returns>GrupoDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrupoDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<GrupoDto>("Grupo no encontrado."));

            return Ok(new ApiResponse<GrupoDto>(result, "Grupo encontrado exitosamente."));
        }

        /// <summary>
        /// Crea un nuevo grupo escolar.
        /// </summary>
        /// <param name="dto">Datos del grupo</param>
        /// <returns>ID del grupo creado envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateGrupoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos del grupo inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Grupo creado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un grupo existente.
        /// </summary>
        /// <param name="id">ID del grupo a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateGrupoDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Grupo actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un grupo.
        /// </summary>
        /// <param name="id">ID del grupo</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Grupo eliminado exitosamente."));
        }
    }
}