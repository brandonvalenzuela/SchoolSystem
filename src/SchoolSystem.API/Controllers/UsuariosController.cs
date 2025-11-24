using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Usuarios;
using SchoolSystem.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de usuarios.
        /// </summary>
        /// <param name="page">Número de página (default: 1)</param>
        /// <param name="size">Tamaño de página (default: 10)</param>
        /// <returns>Lista paginada de UsuarioDto envuelta en ApiResponse</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<UsuarioDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            var response = new ApiResponse<PagedResult<UsuarioDto>>(result, "Listado de usuarios obtenido exitosamente.");
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>UsuarioDto envuelto en ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new ApiResponse<UsuarioDto>("Usuario no encontrado."));

            return Ok(new ApiResponse<UsuarioDto>(result, "Usuario encontrado exitosamente."));
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="dto">Datos del usuario</param>
        /// <returns>ID del usuario creado envuelto en ApiResponse</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos del usuario inválidos."));

            var id = await _service.CreateAsync(dto);
            var response = new ApiResponse<int>(id, "Usuario registrado exitosamente.");

            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateUsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("El ID de la URL no coincide con el ID del cuerpo de la solicitud."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse<int>(id, "Usuario actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina (lógicamente) un usuario.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>ApiResponse indicando éxito</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new ApiResponse<int>(id, "Usuario eliminado exitosamente."));
        }
    }
}