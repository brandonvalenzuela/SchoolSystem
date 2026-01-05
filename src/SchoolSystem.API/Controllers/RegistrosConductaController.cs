using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Application.Common.Models;
using SchoolSystem.Application.Common.Wrappers;
using SchoolSystem.Application.DTOs.Conducta;
using SchoolSystem.Application.Services.Interfaces;
using SchoolSystem.Domain.Constants;
using System.Security.Claims;

namespace SchoolSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación
    public class RegistrosConductaController : ControllerBase
    {
        private readonly IRegistroConductaService _service;

        public RegistrosConductaController(IRegistroConductaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene el historial de conducta paginado.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = Roles.Staff)] // Maestros y Admin pueden ver el historial
        public async Task<ActionResult<ApiResponse<PagedResult<RegistroConductaDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            return Ok(new ApiResponse<PagedResult<RegistroConductaDto>>(result, "Registros de conducta obtenidos exitosamente."));
        }

        /// <summary>
        /// Obtiene un registro de conducta específico.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Staff)]
        public async Task<ActionResult<ApiResponse<RegistroConductaDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<RegistroConductaDto>("Registro de conducta no encontrado."));

            return Ok(new ApiResponse<RegistroConductaDto>(result, "Registro encontrado."));
        }

        /// <summary>
        /// Crea un nuevo reporte de conducta (positivo o negativo).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = Roles.Staff)] // Maestros y Admin pueden reportar
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateRegistroConductaDto dto)
        {
            // 1. Obtener el ID del Usuario desde el Token (Seguridad)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            // Validación de seguridad: Si no hay claim, no está autenticado correctamente
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new ApiResponse<int>("No se pudo identificar al usuario que realiza el reporte."));
            }

            // 2. Sobreescribir el MaestroId en el DTO con el ID del usuario real
            // El servicio luego buscará el perfil de Maestro asociado a este UsuarioId
            dto.MaestroId = userId;

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            // 3. Llamar al servicio
            try
            {
                var id = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<int>(id, "Reporte de conducta registrado exitosamente."));
            }
            catch (Exception ex)
            {
                // Capturar excepciones de negocio (ej. Usuario no es maestro)
                return BadRequest(new ApiResponse<int>(ex.Message));
            }
        }

        /// <summary>
        /// Actualiza un reporte de conducta existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Staff)] // Maestros y Admin pueden actualizar
        public async Task<ActionResult<ApiResponse<int>>> Update(int id, [FromBody] UpdateRegistroConductaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponse<int>("ID mismatch."));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<int>("Datos inválidos."));

            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<int>(id, "Registro actualizado exitosamente."));
        }

        /// <summary>
        /// Elimina un reporte de conducta.
        /// </summary>
        /// <remarks>
        /// Solo los administradores pueden eliminar registros de conducta para evitar manipulación.
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)] // SOLO ADMIN (Directores) pueden borrar historial de conducta
        public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponse<int>(id, "Registro eliminado exitosamente."));
        }
    }
}
