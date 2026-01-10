using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Alumnos;
using SchoolSystem.Application.DTOs.Filtros;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        /// <returns>Lista paginada envuelta en ApiResponse</returns>
        [HttpGet]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<PagedResult<AlumnoDto>>>> GetAll([FromQuery] AlumnoFilterDto filter)
        {
            var result = await _service.GetPagedAsync(filter);
            return Ok(new ApiResponse<PagedResult<AlumnoDto>>(result, "Lista de alumnos obtenida exitosamente."));
        }

        /// <summary>
        /// Obtiene un alumno por su ID.
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>AlumnoDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<AlumnoDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<AlumnoDto>("Alumno no encontrado."));

            return Ok(new ApiResponse<AlumnoDto>(result, "Alumno encontrado exitosamente."));
        }

        /// <summary>
        /// Crea un nuevo alumno.
        /// </summary>
        /// <param name="dto">Datos del alumno</param>
        /// <returns>ID del alumno creado envuelto en ApiResponse</returns>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateAlumnoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos")); // Opcional: Podrías pasar los errores del ModelState

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Alumno creado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un alumno existente.
        /// </summary>
        /// <param name="id">ID del alumno a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateAlumnoDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos"));

            await _service.UpdateAsync(id, dto);

            // Usamos Ok en lugar de NoContent para poder devolver el mensaje de éxito
            return Ok(new ApiResponse<int>(id, "Alumno actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un alumno.
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            // Usamos Ok en lugar de NoContent para poder devolver el mensaje de éxito
            return Ok(new ApiResponse<bool>(true, "Alumno eliminado exitosamente."));
        }
    }
}