using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Grados;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradosController : ControllerBase
    {
        private readonly IGradoService _service;

        public GradosController(IGradoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de grados escolares.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de GradoDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<GradoDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<GradoDto>>(result, "Listado de grados obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un grado por su ID.
        /// </summary>
        /// <param name="id">ID del grado</param>
        /// <returns>GradoDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GradoDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<GradoDto>("Grado no encontrado."));

            return Ok(new ApiResponse<GradoDto>(result, "Grado encontrado exitosamente."));
        }

        /// <summary>
        /// Registra un nuevo grado escolar.
        /// </summary>
        /// <param name="dto">Datos del grado</param>
        /// <returns>ID del grado creado envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateGradoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos del grado inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Grado registrado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un grado existente.
        /// </summary>
        /// <param name="id">ID del grado a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateGradoDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Grado actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un grado escolar.
        /// </summary>
        /// <param name="id">ID del grado</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Grado eliminado exitosamente."));
        }
    }
}