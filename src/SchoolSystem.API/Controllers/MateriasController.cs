using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Materias;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MateriasController : ControllerBase
    {
        private readonly IMateriaService _service;

        public MateriasController(IMateriaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de materias.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de MateriaDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<MateriaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<MateriaDto>>(result, "Listado de materias obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene una materia por su ID.
        /// </summary>
        /// <param name="id">ID de la materia</param>
        /// <returns>MateriaDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MateriaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<MateriaDto>("Materia no encontrada."));

            return Ok(new ApiResponse<MateriaDto>(result, "Materia encontrada exitosamente."));
        }

        /// <summary>
        /// Registra una nueva materia.
        /// </summary>
        /// <param name="dto">Datos de la materia</param>
        /// <returns>ID de la materia creada envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateMateriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos de la materia inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Materia registrada exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza una materia existente.
        /// </summary>
        /// <param name="id">ID de la materia a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateMateriaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Materia actualizada exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) una materia.
        /// </summary>
        /// <param name="id">ID de la materia</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Materia eliminada exitosamente."));
        }
    }
}